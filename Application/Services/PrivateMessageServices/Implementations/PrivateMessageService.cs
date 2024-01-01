using Application.ExceptionMessages;
using Application.Exceptions;
using Application.Models.DTOs.ChatDTOs;
using Application.Models.DTOs.MessageDTOs;
using Application.Services.PrivateMessageServices.Interfaces;
using Application.Services.UserService.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PrivateMessageServices.Implementations
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IPrivateMessageRepository privateMessageRepository;
        private readonly IUserRepository userRepository;
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public PrivateMessageService(
            IPrivateMessageRepository privateMessageRepository,
            IUnitOfWork unitOfWork,
            IAuthenticatedUserService authenticatedUserService,
            IMapper mapper,
            IUserRepository userRepository)
        {
            this.privateMessageRepository = privateMessageRepository;
            this.unitOfWork = unitOfWork;
            this.authenticatedUserService = authenticatedUserService;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<PrivateMessageResponseDto> StorePrivateMessage(string destinationUserId, string textMessage)
        {
            var sourceUserId = authenticatedUserService.GetAuthenticatedUserId();
            var destinationUser = await userRepository.GetUserById(destinationUserId);
            if (destinationUser == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            var message = new PrivateMessage()
            {
                ReceiverId = destinationUserId,
                SenderId = sourceUserId,
                CreationDate = DateTime.Now,
                TextBody = textMessage
            };
            await privateMessageRepository.AddAsync(message);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<PrivateMessageResponseDto>(message);
        }

        public async Task<PrivateMessagesWithPaginationResponseDto> GetPrivateMessages(
            DateTime? pageDate,
            int pageSize,
            string firstUserId,
            string secoundUserId)
        {
            var firstUser = await userRepository.GetUserById(firstUserId);
            var secoundUser = await userRepository.GetUserById(secoundUserId);
            if (firstUser == null || secoundUser == null)
            {
                throw new NotFoundException(UserExceptionMessages.NotFoundUserById);
            }
            var authenticatedUserId = authenticatedUserService.GetAuthenticatedUserId();
            if (authenticatedUserId != firstUserId)
            {
                throw new UnauthorizedException();
            }
            await privateMessageRepository.GetRecentChatsForUser(authenticatedUserId);
            if (pageDate == null)
            {
                pageDate = DateTime.Now;
            }
            var queryResult = await privateMessageRepository.GetPrivateMessagesForPrivateChat((DateTime)pageDate, pageSize, firstUserId, secoundUserId);
            var result = new PrivateMessagesWithPaginationResponseDto
            {
                Messages = mapper.Map<IEnumerable<PrivateMessageResponseDto>>(queryResult.Item1),
                IsThereMore = queryResult.Item2
            };
            return result;
        }

        public async Task<IEnumerable<ChatWithLastMessageResponseDto>> GetRecentChatsForUser(string userId)
        {
            var authenticatedUserId = authenticatedUserService.GetAuthenticatedUserId();
            if (authenticatedUserId != userId)
            {
                throw new UnauthorizedException();
            }
            var queryResult = await privateMessageRepository.GetRecentChatsForUser(userId);

            return mapper.Map<IEnumerable<ChatWithLastMessageResponseDto>>(queryResult);
        }
    }
    
}
