using fit_and_fuel.Model;
using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class PostDto
    {

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }


        public string ImageUrl { get; set; }

    }
}
