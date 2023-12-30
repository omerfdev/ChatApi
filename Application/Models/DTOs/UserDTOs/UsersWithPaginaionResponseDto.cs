using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.UserDTOs
{
    public class UsersWithPaginationResponseDto
    {
        public IEnumerable<UserResponseDto> users { get; set; } = new List<UserResponseDto>();
        public int numOfPages { get; set; }
        public int currentPage { get; set; }
    }
}
