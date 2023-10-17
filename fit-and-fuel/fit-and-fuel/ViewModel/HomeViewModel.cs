using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.ViewModel
{
    public class HomeViewModel
    {
        public List <Post> posts { get; set; }
        public List<Nutritionist> nutritionists { get; set; }


		// for comments:
		public List<Comment> Comments { get; set; }
		public CommentDto NewComment { get; set; }
	}
}
