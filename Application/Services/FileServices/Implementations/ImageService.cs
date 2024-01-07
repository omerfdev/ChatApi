using Application.Services.FileServices.Interfaces;
using Application.Services.UserService.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.FileServices.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imagesRepository;
        private readonly IUserRepository userRepository;
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly IUnitOfWork unitOfWork;

        public ImageService(
            IImageRepository imagesRepository,
            IUnitOfWork unitOfWork,
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository)
        {
            this.imagesRepository = imagesRepository;
            this.unitOfWork = unitOfWork;
            this.authenticatedUserService = authenticatedUserService;
            this.userRepository = userRepository;
        }

        public async Task<string> UploadImage(string imagePath, string userid)
        {
            try
            {
                // Read the file content
                byte[] fileBytes;
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                }

                // Save the file content to MongoDB
                var imagesEntity = new Images
                {
                    ImagePath = fileBytes.ToString(),
                    userId = userid,
                    
                    // Other properties as needed
                };
               
                await imagesRepository.AddAsync(imagesEntity);

                // Commit the unit of work if needed
                await unitOfWork.SaveChangesAsync();

                // Return the Id of the created entity
                return imagesEntity.Id;
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Failed to upload image.", ex);
            }
        }

        public async Task DeleteImage(string imagesId)
        {
            try
            {
                // Retrieve the Images entity based on the fileId
                var imagesEntity = await imagesRepository.GetByIdAsync(imagesId);

                // Check if the entity exists
                if (imagesEntity != null)
                {
                    // Delete the corresponding Images entity from the repository
                    await imagesRepository.DeleteAsync(imagesEntity.Id);

                    // Commit the unit of work if needed
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    // Handle the case where the file/entity does not exist
                    throw new Exception("File not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly
                throw new Exception("Failed to delete image.", ex);
            }
        }
    }
}
