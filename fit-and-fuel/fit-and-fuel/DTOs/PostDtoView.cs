using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class PostDtoView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsImproved { get; set; } = false;
        public DateTime Time { get; set; }
        public int NutritionistId { get; set; }
        
        public int? NumOfLikes
        {
            get; set;
        }

        //public List<Comment>? Comments { get; set; }
    }
}
