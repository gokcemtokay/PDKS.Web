using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{


    public class FileUploadService : IFileUploadService
    {
        private readonly string _uploadBasePath = "wwwroot/uploads";

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Dosya boş olamaz.");

            // Klasör oluştur
            var uploadPath = Path.Combine(_uploadBasePath, folderName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Benzersiz dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Relative path döndür
            return $"/uploads/{folderName}/{fileName}";
        }

        public async Task<List<string>> UploadMultipleFilesAsync(IFormFileCollection files, string folderName)
        {
            var uploadedFiles = new List<string>();

            foreach (var file in files)
            {
                var filePath = await UploadFileAsync(file, folderName);
                uploadedFiles.Add(filePath);
            }

            return uploadedFiles;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidFileType(IFormFile file, List<string> allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        public bool IsValidFileSize(IFormFile file, long maxSizeMB)
        {
            var maxSizeBytes = maxSizeMB * 1024 * 1024;
            return file.Length <= maxSizeBytes;
        }
    }
}