using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface ILike
    {
        Task addLike(LikeDto likeDto, int UserId);

        Task<List<Like>> getAll(int UserId);

        Task<List<LikeDtoView>> getAllDto(int UserId);

        Task<Like> getLike(int id);
        Task<LikeDtoView> getLikeDto(int id);
        Task deleteLike(int id, int UserId);

        Task<List<Like>> GetMyLikes(int userId);
        Task<List<LikeDtoView>> GetMyLikesDto(int userId);
       
    }
}
