using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IComment
    {
        Task PostCommnet(CommentDto commentDto,int UserId);

        Task<List<Comment>> GetAll(int UserId);

        Task<List<CommentDtoView>> GetAllDto(int UserId);

        Task<Comment> GetComment(int id);
        Task<CommentDtoView> GetCommentDto(int id);

        Task DeleteComment(int id, int UserId);

        Task PutComment(int id , CommentDto commentDto, int UserId);
        // for comment interface (without userId):
		Task PostCommnet(CommentDto commentDto);

	}
}
