using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using hrmApp.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace hrmApp.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public FileService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }
        private string docPath;
        private string rootPath;
        private string[] allowedExtensions = new[] { ".pdf" };

        #region Task<string> SaveFileAsync(IFormFile formFile, string folderName)

        public async Task<string> SaveFileAsync(IFormFile formFile, string filePath)
        {
            var fullPath = GetFullPath(filePath);

            Directory.CreateDirectory(fullPath);

            string extension = Path.GetExtension(formFile.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                throw new BadImageFormatException();
            }
            var newFileName = $"{Guid.NewGuid()}{extension}";

            using (var fileStream = new FileStream(Path.Combine(fullPath, newFileName), FileMode.Create, FileAccess.Write))
            {
                try
                {
                    await formFile.CopyToAsync(fileStream);
                }
                catch (Exception ex)
                {
                    Log.Information($"Uploaded file save failed. {ex}");
                    throw ex;
                }
            }

            Log.Information($"Uploaded file saved: {filePath}/{newFileName}");
            return $"{filePath}/{newFileName}";
        }

        #endregion

        #region DeleteFile(string filePath)
        public bool DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var fullPath = GetFullPath(filePath);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                        Log.Information($"File deleted. {fullPath}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Information($"File delete failed. {ex}");
                        return false;
                        // throw;
                    }
                }
                Log.Information($"DeleteFile file not exist. {fullPath} ");
                return false;
            }
            Log.Information($"DeleteFile filePath is Null or Emplty. ");
            return false;
            // throw new System.NotImplementedException();
        }

        #endregion

        #region GetFullPath(string filePath)
        public string GetFullPath(string filePath)
        {
            docPath = _configuration.GetSection("DocumentStorage").GetValue<string>("Path");
            rootPath = _env.WebRootPath;
            return Path.Combine(rootPath, docPath, filePath);
        }
        #endregion

        #region ConvertValidFileName(string fileName)
        public string ConvertValidFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        #endregion

    }
}
