using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    public class CommentService : IComment
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context )
        {
            _context = context;
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

       public async Task<List<Comment>> GetAll(int UserId)

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

        public async Task<List<CommentDtoView>> GetAllDto(int UserId)
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

        public async Task PostCommnet(CommentDto commentDto, int UserId)
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


        /// <summary>
        /// Updates the content of a comment with new details.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="commentDto">The updated details of the comment.</param>

         public async Task PutComment(int id, CommentDto commentDto, int UserId)

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
    }
}
