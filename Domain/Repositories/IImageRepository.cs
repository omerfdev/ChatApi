using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IImageRepository
    {
        Task AddAsync(Images images);
        Task DeleteAsync(string imagesId);
        Task<Images> GetByIdAsync(string imagesId);
    }
}
