using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // POST: api/FileUpload/Single
        [HttpPost("Single")]
        public async Task<ActionResult<object>> UploadSingleFile(IFormFile file, [FromQuery] string folderName = "documents")
        {
            // Dosya kontrolü
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Dosya seçilmedi." });

            // Dosya tipi kontrolü
            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
            if (!_fileUploadService.IsValidFileType(file, allowedExtensions))
                return BadRequest(new { message = "Geçersiz dosya tipi. İzin verilen: jpg, jpeg, png, pdf, doc, docx, xls, xlsx" });

            // Dosya boyutu kontrolü (10MB)
            if (!_fileUploadService.IsValidFileSize(file, 10))
                return BadRequest(new { message = "Dosya boyutu 10MB'dan büyük olamaz." });

            try
            {
                var filePath = await _fileUploadService.UploadFileAsync(file, folderName);

                return Ok(new
                {
                    message = "Dosya başarıyla yüklendi.",
                    filePath = filePath,
                    fileName = file.FileName,
                    fileSize = file.Length
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Dosya yüklenirken hata oluştu.", error = ex.Message });
            }
        }

        // POST: api/FileUpload/Multiple
        [HttpPost("Multiple")]
        public async Task<ActionResult<object>> UploadMultipleFiles(IFormFileCollection files, [FromQuery] string folderName = "documents")
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { message = "Dosya seçilmedi." });

            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
            var uploadedFiles = new List<object>();
            var errors = new List<string>();

            foreach (var file in files)
            {
                // Dosya kontrolü
                if (!_fileUploadService.IsValidFileType(file, allowedExtensions))
                {
                    errors.Add($"{file.FileName}: Geçersiz dosya tipi.");
                    continue;
                }

                if (!_fileUploadService.IsValidFileSize(file, 10))
                {
                    errors.Add($"{file.FileName}: Dosya boyutu 10MB'dan büyük.");
                    continue;
                }

                try
                {
                    var filePath = await _fileUploadService.UploadFileAsync(file, folderName);
                    uploadedFiles.Add(new
                    {
                        fileName = file.FileName,
                        filePath = filePath,
                        fileSize = file.Length
                    });
                }
                catch (Exception ex)
                {
                    errors.Add($"{file.FileName}: {ex.Message}");
                }
            }

            return Ok(new
            {
                message = "Yükleme tamamlandı.",
                uploadedFiles = uploadedFiles,
                errors = errors
            });
        }

        // DELETE: api/FileUpload
        [HttpDelete]
        public async Task<ActionResult> DeleteFile([FromQuery] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest(new { message = "Dosya yolu belirtilmedi." });

            var result = await _fileUploadService.DeleteFileAsync(filePath);

            if (result)
                return Ok(new { message = "Dosya başarıyla silindi." });

            return NotFound(new { message = "Dosya bulunamadı." });
        }
    }
}