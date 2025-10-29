using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PDKS.Business.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task<List<string>> UploadMultipleFilesAsync(IFormFileCollection files, string folderName);
        Task<bool> DeleteFileAsync(string filePath);
        bool IsValidFileType(IFormFile file, List<string> allowedExtensions);
        bool IsValidFileSize(IFormFile file, long maxSizeMB);
    
        //Task<IEnumerable<FileUploadListDTO>> GetBySirketAsync(int sirketId);
}
}
