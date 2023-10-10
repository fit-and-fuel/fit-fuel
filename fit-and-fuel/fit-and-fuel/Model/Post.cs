using fit_and_fuel.Interfaces;

namespace fit_and_fuel.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool? IsImproved { get; set; } = false;
        public DateTime Time { get; set; }
        public int NutritionistId { get; set; }
        public Nutritionist nutritionist { get; set; }

        public List<Like>? likes { get; set; }

        public int? NumOfLikes
        {
            get { return likes?.Count ?? 0; } 
        }

        public List<Comment>? Comments { get; set; }


    }
}
