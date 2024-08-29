using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.FileManagement
{
    public class FileManager
    {
        private readonly string _rootPath;

        public FileManager(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.", nameof(file));

            string uploadsFolder = Path.Combine(_rootPath, folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }

        public bool DeleteFile(string relativeFilePath)
        {
            if (string.IsNullOrEmpty(relativeFilePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(relativeFilePath));

            string filePath = Path.Combine(_rootPath, relativeFilePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}

