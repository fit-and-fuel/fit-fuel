namespace fit_and_fuel.DTOs.View.ViewAvilableTme
{
    public class ViewAvilableTime
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }
    }
}
