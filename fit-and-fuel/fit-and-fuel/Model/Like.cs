namespace fit_and_fuel.Model
{
    public class Like
    {
        public int Id { get; set; }

        public Post post { get; set; }  

        public int postId { get; set; }

        public int patientId { get; set; }

        public Patient Patient { get; set; }
    }
}
