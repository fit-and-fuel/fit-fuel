namespace fit_and_fuel.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public Post post { get; set; }

        public int postId { get; set; }

        public int patientId { get; set; }

        public Patient Patient { get; set; }

        public string content { get; set; } = string.Empty;
    }
}
