using Application.Models.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Interfaces
{
    public interface IUserRetrievalService
    {
        Task<UsersWithPaginationResponseDto> GetUsers(int pageNumber, int pageSize, string searchText = null);
        Task<UserResponseDto> GetUserById(int userId);
    }
}
