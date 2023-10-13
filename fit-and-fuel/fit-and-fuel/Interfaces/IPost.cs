﻿using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IPost
    {
        Task<List<Post>> GetAll();
        Task<List<PostDtoView>> GetAllDto();
        Task<Post> GetById(int id);
        Task<PostDtoView> GetByIdDto(int id);
        Task<Post> Post(int UserId,PostDto PostDto);
        Task<bool> ImprovedPost (int PostId);
        Task Put(int id, PostDto PostDto, int postId);
        Task Delete(int id,int UserId);
		Task<int> Count();
	}
}
