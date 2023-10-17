using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class CommentService : IComment
    {
        private readonly AppDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public CommentService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>

        public async Task DeleteComment(int id, int UserId)
        {
            var comment = await GetComment(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Retrieves a list of all comments.
        /// </summary>
        /// <returns>A list of all comments.</returns>

       public async Task<List<Comment>> GetAll(string UserId)

        {
            var patient = await _context.Patients.Where(p=>p.UserId == UserId).FirstOrDefaultAsync();
            if (patient != null)
            {
                var comments = await _context.Comment
                 .Where(c => c.patientId == patient.Id)
                 .ToListAsync();
                return comments;
            }
            throw new Exception("we cant comment");
            
        }

        public Task<List<Comment>> GetAll(int UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CommentDtoView>> GetAllDto(string UserId)
        {
            var comments = await GetAll(UserId);
            var commentToReturn = comments.Select(comment=> new CommentDtoView
            {
                Id = comment.Id,
                postId = comment.postId,
                patientId = comment.patientId,
                content = comment.content
            }).ToList();
            return commentToReturn;

        }

        public Task<List<CommentDtoView>> GetAllDto(int UserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <returns>The comment with the specified ID.</returns>

        public async Task<Comment> GetComment(int id)
        {
           var comment = await _context.Comment.Where(c=>c.Id==id).FirstOrDefaultAsync();
            return comment;

        }

        public async Task<CommentDtoView> GetCommentDto(int id)
        {
            var comment = await GetComment(id);
            var commentToReturn = new CommentDtoView
            {
                Id = comment.Id,
                postId = comment.postId,
                patientId = comment.patientId,
            };
            return commentToReturn;
        }


        /// <summary>
        /// Creates a new comment with the specified details.
        /// </summary>
        /// <param name="commentDto">The details of the comment to create.</param>

        public async Task PostCommnet(CommentDto commentDto, string UserId)
        {
            var patient = await _context.Patients.Where(p => p.UserId == UserId).FirstOrDefaultAsync();
            var comment = new Comment()
            {
                patientId = patient.Id,
                postId = commentDto.postId,
                content = commentDto.content

            };
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public Task PostCommnet(CommentDto commentDto, int UserId)
        {
            throw new NotImplementedException();
        }


        // for interface:
		public async Task PostCommnet(CommentDto commentDto)
		{
			string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var patient = await _context.Patients.Where(p => p.UserId == userId).FirstOrDefaultAsync();
			var comment = new Comment()
			{
				patientId = patient.Id,
				postId = commentDto.postId,
				content = commentDto.content

			};
			await _context.Comment.AddAsync(comment);
			await _context.SaveChangesAsync();

			//         var patient = await _context.Patients.Where(p => p.UserId == userId).FirstOrDefaultAsync();

			//if (patient == null)
			//         {
			//    var nutritionist = await _context.Nutritionists.Where(p => p.UserId == userId).FirstOrDefaultAsync();
			//	var nComment = new Comment()
			//	{
			//		patientId = nutritionist.Id,
			//		postId = commentDto.postId,
			//		content = commentDto.content

			//	};
			//	await _context.Comment.AddAsync(nComment);
			//	await _context.SaveChangesAsync();
			//}
			//         else
			//         {
			//	var comment = new Comment()
			//	{
			//		patientId = patient.Id,
			//		postId = commentDto.postId,
			//		content = commentDto.content

			//	};
			//	await _context.Comment.AddAsync(comment);
			//	await _context.SaveChangesAsync();
			//}
		}


		/// <summary>
		/// Updates the content of a comment with new details.
		/// </summary>
		/// <param name="id">The ID of the comment to update.</param>
		/// <param name="commentDto">The updated details of the comment.</param>

		public async Task PutComment(int id, CommentDto commentDto, string UserId)

        {
            var patient = await _context.Patients.Where(p => p.UserId == UserId).FirstOrDefaultAsync();

            var newComment = await _context.Comment.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (patient.Id == newComment.patientId)
            {
                newComment.content = commentDto.content;
            }
            else
            {
                throw new Exception("this Comment not belong to you");
            }
           

            await _context.SaveChangesAsync();


        }

        public Task PutComment(int id, CommentDto commentDto, int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
