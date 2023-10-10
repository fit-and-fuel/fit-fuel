using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class CommentDtoView
    {
        public int Id { get; set; }

        public int postId { get; set; }

        public int patientId { get; set; }

        public string content { get; set; } 
    }
}
