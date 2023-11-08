using System.ComponentModel.DataAnnotations;

namespace QandA.Models
{
    public class AnswerPostRequest
    {
        [Required]
        public int? QuestionId { get; set; }
        [Required(ErrorMessage = "Please include some content for the answer")]
        public string Content { get; set; }
    }
}