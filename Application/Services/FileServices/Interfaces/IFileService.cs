using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FileServices.Interfaces
{
    public interface IFileService
    {
        Task<string> StoreImageToLocalFolder(IFormFile file);
        void DeleteFile(string filePath);
    }
}
