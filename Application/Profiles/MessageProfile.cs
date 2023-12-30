using Application.Models.DTOs.MessageDTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile() { CreateMap<PrivateMessage, PrivateMessageResponseDto>(); }
    }
}
