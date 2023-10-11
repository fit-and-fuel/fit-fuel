namespace fit_and_fuel.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public bool ReadNotfication { get; set; } = false;
    }
}
