using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class PostService : IPost
    {
        private readonly AppDbContext _context;

        private INotification _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;



        public PostService(AppDbContext context, INotification notificationService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;

            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<int> Count()
        {
            return await _context.Posts.CountAsync();
        }

        /// <summary>
        /// Deletes a post with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <param name="UserId">The ID of the user attempting the deletion.</param>

        public async Task Delete(int id, string UserId)
        {
            var post = await _context.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();
            var nut = await _context.Nutritionists.FirstOrDefaultAsync(n => n.UserId == UserId);
            if (post.NutritionistId != nut.Id)
            {
                throw new Exception("you can`t Delete post not belong to you why you want to do that?");
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var post = await _context.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        public Task Delete(int id, int UserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a list of all improved posts.
        /// </summary>
        /// <returns>A list of improved posts.</returns>

        public async Task<List<Post>> GetAll()
        {
            var Allposts = await _context.Posts
            .Where(p => p.IsImproved == true)
            .Include(p => p.nutritionist)
            .ToListAsync();
            return Allposts;
        }


        public async Task<List<PostDtoView>> GetAllDto()
        {
            var posts = await GetAll();
            var postsToReturn = posts.Select(post => new PostDtoView
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                ImageUrl = post.ImageUrl,
                IsImproved = post.IsImproved,
                Time = post.Time,
                NutritionistId = post.NutritionistId,
                NumOfLikes = post.NumOfLikes,
            }).ToList();
            return postsToReturn;
        }

        /// <summary>
        /// Retrieves a post with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the post to retrieve.</param>
        /// <returns>The retrieved post.</returns>

        public async Task<Post> GetById(int id)
        {
            var post = await _context.Posts
            .Include(p => p.nutritionist)
            .Include(p => p.likes)
            .Include(p => p.Comments)
            .ThenInclude(n => n.Patient)
            .Where(p => p.Id == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<PostDtoView> GetByIdDto(int id)
        {
            var post = await GetById(id);
            var postsToReturn = new PostDtoView
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                ImageUrl = post.ImageUrl,
                IsImproved = post.IsImproved,
                Time = post.Time,
                NutritionistId = post.NutritionistId,
                NumOfLikes = post.NumOfLikes,
            };
            return postsToReturn;
        }

        /// <summary>
        /// Marks a post as improved and sends a notification to the nutritionist.
        /// </summary>
        /// <param name="PostId">The ID of the post to mark as improved.</param>
        /// <returns>True if the post is marked as improved and the notification is sent, otherwise false.</returns>

        public async Task<bool> ImprovedPost(int PostId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == PostId);
            post.IsImproved = true;
            await _context.SaveChangesAsync();
            if (post.IsImproved == true)
            {
                var nut = await _context.Nutritionists.FirstOrDefaultAsync(n => n.Id == post.NutritionistId);



                var content = new NotificationDto()
                {
                    Content = $"Your Post with Title : {post.Title} Improved Now"
                };

                await _notificationService.SendNotification(nut.UserId.ToString(), content);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a new post and sends a notification for improvement.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist creating the post.</param>
        /// <param name="PostDto">DTO containing post information.</param>
        /// <returns>The newly created post.</returns>

        public async Task<Post> Post(PostDto PostDto, IFormFile file)
        {
            var imageUrl = await UploadFile(file);


            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var nut = await _context.Nutritionists.FirstOrDefaultAsync(n => n.UserId == userId);

            var newPost = new Post();

            newPost.Title = PostDto.Title;
            newPost.Description = PostDto.Description;
            newPost.ImageUrl = imageUrl;
            newPost.Time = DateTime.Now;
            newPost.NutritionistId = nut.Id;

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            var content = new NotificationDto()
            {
                Content = $"New Post Need To Improve with PostId : {newPost.Id}"
            };
            try
            {
                await _notificationService.SendNotification("1", content);
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
                // For testing purposes, you might want to log the exception or throw it again
                // For example: throw ex;
            }
            return newPost;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var URL = "https://ecommerceprojectimages.blob.core.windows.net/images/noimage.png";
            if (file != null)
            {
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(_configuration.GetConnectionString("StorageAccount"), "images");

                await blobContainerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

                using var fileStream = file.OpenReadStream();

                BlobUploadOptions blobUploadOptions = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                };

                if (!blobClient.Exists())
                {
                    await blobClient.UploadAsync(fileStream, blobUploadOptions);
                }
                URL = blobClient.Uri.ToString();
            }
            return URL;
        }



        public Task<Post> Post(int UserId, PostDto PostDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing post's information.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist updating the post.</param>
        /// <param name="PostDto">DTO containing updated post information.</param>
        /// <param name="postId">The ID of the post to update.</param>


        public async Task Put(string UserId, PostDto PostDto, int postId)
        {
            var newPost = await _context.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
            var nut = await _context.Nutritionists.FirstOrDefaultAsync(n => n.UserId == UserId);
            if (nut != null && newPost.NutritionistId != nut.Id)
            {
                throw new Exception("you can`t change post not belong to you");
            }
            newPost.Title = PostDto.Title;
            newPost.Description = PostDto.Description;
            newPost.ImageUrl = PostDto.ImageUrl;
            newPost.Time = DateTime.Now;


            await _context.SaveChangesAsync();
        }

        public Task Put(int id, PostDto PostDto, int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> GetMyPosts()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nutritionist = await _context.Nutritionists.Where(n => n.UserId == userId).FirstOrDefaultAsync();

            var Allposts = await _context.Posts
            .Where(p => p.NutritionistId == nutritionist.Id)
            //.Include(p => p.nutritionist)
            .ToListAsync();

            return Allposts;

        }

        public async Task<List<Post>> GetAllPosts()
        {
            var AllPosts = await _context.Posts
                .OrderByDescending(p => p.Id)
                .Include(p => p.nutritionist)
                .ToListAsync();

            return AllPosts;
        }

    }
}
