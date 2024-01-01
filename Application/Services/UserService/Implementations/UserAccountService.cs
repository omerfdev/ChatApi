using Application.ExceptionMessages;
using Application.Exceptions;
using Application.Models.DTOs.UserDTOs;
using Application.Services.UserService.Interfaces;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAuthenticatedUserService authenticatedUserService;
        //
     

        public UserAccountService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAuthenticatedUserService authenticatedUserService
           )
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.authenticatedUserService = authenticatedUserService;
         
        }


        public async Task<UserResponseDto> RegisterUserAsync(UserRequestDto userRequestDto)
        {
            if (userRepository.CheckIfUsernameExists(userRequestDto.Username))
            {
                throw new NotFoundException(UserExceptionMessages.UsernameAlreadyExsist);
            }
            PasswordHashing.HashPassword(userRequestDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User()
            {
                Username = userRequestDto.Username.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
       
            return mapper.Map<UserResponseDto>(user);
            
        }

        public async Task<string> LoginUserAsync(UserRequestDto userRequestDto)
        {
            var user = await userRepository.GetUserByUsername(userRequestDto.Username.ToLower());
            if (user == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserByUsername);
            }
            if (!PasswordHashing.VerifyPassword(userRequestDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new BadRequestException(UserExceptionMessages.IncorrectPassword);
            }
            var token = TokenGenerator.Generate(user);
            return token;
        }

        public async Task<UserResponseDto> GetUserByJwtTokenAsync()
        {
            var userId = authenticatedUserService.GetAuthenticatedUserId();
            var user = await userRepository.GetUserById(userId);
            return mapper.Map<UserResponseDto>(user);
        }

        public async Task ChangePasswordAsync(string userId, ChangePasswordRequestDto changePasswordDto)
        {
            var authenticatedUserId = authenticatedUserService.GetAuthenticatedUserId();
            if (authenticatedUserId != userId)
            {
                throw new UnauthorizedException();
            }
            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            if (!PasswordHashing.VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new BadRequestException(UserExceptionMessages.IncorrectPassword);
            }
            PasswordHashing.HashPassword(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeUserAboutAsync(string userId, string newAbout)
        {
            var authenticatedUserId = authenticatedUserService.GetAuthenticatedUserId();
            if (authenticatedUserId != userId)
            {
                throw new UnauthorizedException();
            }
            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            user.About = newAbout;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeThemeModeAsync(string userId, bool isDarkMode)
        {
            var authenticatedUserId = authenticatedUserService.GetAuthenticatedUserId();
            if (authenticatedUserId != userId)
            {
                throw new UnauthorizedException();
            }
            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            user.IsDarkTheme = isDarkMode;
            await unitOfWork.SaveChangesAsync();
        }
    }
}
