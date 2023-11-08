export const server = 'https://localhost:7212';

export const webAPIUrl = `${server}/api`;

export const authSettings = {
  domain: 'dev-11jr6xojfdk6m1q1.us.auth0.com',
  clientId: 'c9ArobOg4yuVynma8yrB6ZAnzfOGV9kX',
  authorizationParams: {
    redirect_uri: window.location.origin + '/signin-callback',
  },
  //redirect_uri: window.location.origin + '/signin-callback',
  scope: 'openid profile QandAAPI email',
  audience: 'https://qanda',
};
