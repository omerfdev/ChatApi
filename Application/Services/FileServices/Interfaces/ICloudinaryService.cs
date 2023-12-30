using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FileServices.Interfaces
{
    public interface ICloudinaryService
    {
        Task DeleteImageFromCloudinary(string CloudinaryIdentifier);
        Task<Tuple<string, string>> UploadImageToCloudinary(string imagePath);
    }
}
