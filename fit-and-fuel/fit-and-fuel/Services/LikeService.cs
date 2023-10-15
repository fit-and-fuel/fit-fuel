using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    /// <summary>
    /// Service class responsible for handling operations related to likes on posts.
    /// </summary>
    public class LikeService : ILike
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Adds a new like to a post.
        /// </summary>
        /// <param name="likeDto">The DTO containing like information.</param>

         public async Task addLike(LikeDto likeDto, string UserId)

        {
            var patient = await _context.Patients.Where(p => p.UserId == UserId).FirstOrDefaultAsync();
            var like = new Like()
            {
                patientId = patient.Id,
                postId = likeDto.postId
            };

           await _context.Like.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public Task addLike(LikeDto likeDto, int UserId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes a like based on its ID.
        /// </summary>
        /// <param name="id">The ID of the like to be deleted.</param>

        public async Task deleteLike(int id, string UserId)

        {
            var patient = await _context.Patients.Where(p => p.UserId == UserId).FirstOrDefaultAsync();
            var like = await _context.Like.Where(l => l.Id == id).FirstOrDefaultAsync();
            if (like.patientId == patient.Id) 
            { 
            _context.Like.Remove(like);
            }
            await _context.SaveChangesAsync();
        }

        public Task deleteLike(int id, int UserId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Retrieves a list of all likes.
        /// </summary>
        /// <returns>A list of all likes.</returns>

        public async Task<List<Like>> getAll(string UserId)

        {
            var patient = await _context.Patients.Where(p=>p.UserId == UserId).FirstOrDefaultAsync();
            var likes = await _context.Like
                .Where(l=>l.patientId == patient.Id)
                .ToListAsync();
            return likes;
        }

        public Task<List<Like>> getAll(int UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LikeDtoView>> getAllDto(string UserId)
        {
            var likes = await getAll(UserId);
            var likesToReturn = likes.Select(like => new LikeDtoView
            {
                Id = like.Id,
                postId = like.postId,
                patientId = like.patientId,
            }).ToList();
            return likesToReturn;
        }

        public Task<List<LikeDtoView>> getAllDto(int UserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a specific like based on its ID.
        /// </summary>
        /// <param name="id">The ID of the like to retrieve.</param>
        /// <returns>The retrieved like.</returns>

        public async Task<Like> getLike(int id)
        {
            var like = await _context.Like.Where(l => l.Id == id).FirstOrDefaultAsync();
            return like;
        }

        public async Task<LikeDtoView> getLikeDto(int id)
        {
            var like = await getLike(id);
            var likesToReturn =  new LikeDtoView
            {
                Id = like.Id,
                postId = like.postId,
                patientId = like.patientId,
            };
            return likesToReturn;

        }

        /// <summary>
        /// Retrieves a list of likes associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of likes associated with the user.</returns>

        public async Task<List<Like>> GetMyLikes(string userId)
        {
            var patient = await _context.Patients.Where(p => p.UserId == userId).FirstOrDefaultAsync();
            var likes = await _context.Like.Where(l => l.patientId == patient.Id).ToListAsync();
            return likes;
        }

        public Task<List<Like>> GetMyLikes(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LikeDtoView>> GetMyLikesDto(string userId)
        {
            var likes = await GetMyLikes(userId);
            var likesToReturn = likes.Select(like => new LikeDtoView
            {
                Id = like.Id,
                postId = like.postId,
                patientId = like.patientId,
            }).ToList();
            return likesToReturn;
        }

        public Task<List<LikeDtoView>> GetMyLikesDto(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
