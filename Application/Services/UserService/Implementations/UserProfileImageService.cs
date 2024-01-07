using Application.ExceptionMessages;
using Application.Exceptions;
using Application.Services.FileServices.Interfaces;
using Application.Services.UserService.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.UserService.Implementations
{
    public class UserProfileImageService : IUserProfileImageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        private readonly IImageService imageService;
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly IUserRepository userRepository;

        public UserProfileImageService(
            IUnitOfWork unitOfWork,
            IFileService fileService,
            IImageService imageService,
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
            this.fileService = fileService;
            this.authenticatedUserService = authenticatedUserService;
            this.userRepository = userRepository;
        }

        public async Task ChangeProfilePictureAsync(string userId, IFormFile image)
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
            var imageLocalPath = await fileService.StoreImageToLocalFolder(image);
            if (user.Image == null)
            {
                throw new BadRequestException(UserExceptionMessages.DoNotHaveProfilePicture);
            }
            await imageService.DeleteImage(user.Image.Id);
            var uploadResult = imageService.UploadImage(imageLocalPath, user.Id);
            fileService.DeleteFile(imageLocalPath);

            user.Image = new Images()
            {
                ImagePath = uploadResult.ToString(),
                // Other properties as needed
            };
            await unitOfWork.SaveChangesAsync();
        }

        public async Task AddProfilePictureAsync(string userId, IFormFile image)
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
            if (user.Image != null)
            {
                throw new BadRequestException(UserExceptionMessages.AlreadyHaveProfilePicture);
            }
            var imageLocalPath = await fileService.StoreImageToLocalFolder(image);
            var uploadResult = await imageService.UploadImage(imageLocalPath,user.Id);
            
            user.Image = new Images()
            {
                Id = uploadResult,
                ImagePath = imageLocalPath,
                userId = user.Id
                // Other properties as needed
            };
            await unitOfWork.SaveChangesAsync();

        }

        public async Task DeleteProfilePictureAsync(string userId)
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
            if (user.Image == null)
            {
                throw new BadRequestException(UserExceptionMessages.DoNotHaveProfilePicture);
            }
            await imageService.DeleteImage(user.Image.Id);
            user.Image = null;
            await unitOfWork.SaveChangesAsync();
        }
    }
}
