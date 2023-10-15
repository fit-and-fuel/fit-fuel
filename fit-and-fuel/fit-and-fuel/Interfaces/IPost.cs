using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IPost
    {
        Task<List<Post>> GetAll();
        Task<List<PostDtoView>> GetAllDto();
        Task<Post> GetById(int id);
        Task<PostDtoView> GetByIdDto(int id);
        Task<Post> Post(PostDto PostDto, IFormFile file);
        Task<bool> ImprovedPost (int PostId);
        Task Put(int id, PostDto PostDto, int postId);
        Task Delete(int id,int UserId);
		Task<int> Count();

		Task<List<Post>> GetMyPosts();
		Task<List<Post>> GetAllPosts();

	}
}
