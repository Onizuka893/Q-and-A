using DbUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using QandA.Data;
using QandA.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
EnsureDatabase.For.SqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .SqlDatabase(connectionString, null)
        .WithScriptsEmbeddedInAssembly(
                   System.Reflection.Assembly.GetExecutingAssembly()
                          )
        .WithTransaction()
        .LogToConsole()
        .Build();

if (upgrader.IsUpgradeRequired())
{
    upgrader.PerformUpgrade();
}

builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IQuestionCache, QuestionCache>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IAuthorizationHandler, MustBeQuestionAuthorHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddAuthorization(options => options.AddPolicy("MustBeQuestionAuthor", policy
    => policy.Requirements.Add(new MustBeQuestionAuthorRequirement())));


CorsPolicyBuilder corsBuilder = new();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.WithOrigins(builder.Configuration["FrontendURL"]);

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", corsBuilder.Build()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
