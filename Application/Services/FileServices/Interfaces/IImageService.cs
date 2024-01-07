using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FileServices.Interfaces
{
    public interface IImageService
    {
        Task DeleteImage(string Images);
        Task<string> UploadImage(string imagePath,string userId);
    }
}
