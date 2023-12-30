using Application.ExceptionMessages;
using Application.Exceptions;
using Application.Models.DTOs.UserDTOs;
using Application.Services.UserService.Interfaces;
using AutoMapper;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Implementations
{
    public class UserRetrievalService : IUserRetrievalService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserRetrievalService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UsersWithPaginationResponseDto> GetUsers(
            int pageNumber,
            int pageSize,
            string searchText = null)
        {
            if (searchText != null)
            {
                searchText = searchText.Trim().ToLower();
            }
            var result = await userRepository.GetUsers(pageNumber, pageSize, searchText);
            if (result == null)
            {
                throw new BadRequestException(PaginationExceptionMessages.EnteredPageNumberExceedPagesCount);
            }
            var response = new UsersWithPaginationResponseDto
            {
                users = mapper.Map<IEnumerable<UserResponseDto>>(result.Item1),
                numOfPages = result.Item2,
                currentPage = pageNumber
            };
            return response;
        }

        public async Task<UserResponseDto> GetUserById(int userId)
        {
            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            return mapper.Map<UserResponseDto>(user);
        }
    }
}
