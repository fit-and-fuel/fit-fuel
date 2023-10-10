namespace fit_and_fuel.DTOs
{
    public class CommentDto
    {
        public int postId { get; set; }

        public int patientId { get; set; }

        public string content { get; set; } = string.Empty;

    }
}
