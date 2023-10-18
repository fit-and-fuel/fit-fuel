using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
	public class CommentController : Controller
	{
		private readonly IComment _comment;

        public CommentController(IComment comment)
        {
			_comment = comment;
        }

        public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddComment()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddComment(CommentDto commentDto)
		{
			await _comment.PostCommnet(commentDto);

			return RedirectToAction("Post", "Home", new { id = commentDto.postId });
		}

	}
}
