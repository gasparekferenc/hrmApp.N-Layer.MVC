using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace hrmApp.Core.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile, string folderName);
        // Task<IFormFile> GetFileAsync(string filePath);
        bool DeleteFile(string filePath);
        string GetFullPath(string filePath);
        string ConvertValidFileName(string fileName);
    }
}
