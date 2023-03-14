using Application.Interfaces;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Headers;

namespace WebAPI.Controllers
{
    public class TrainingMaterialsController : BaseController
    {
        private readonly ITrainingMaterialService _trainingMaterialService;

        private IFileProvider _fileProvider;

        public TrainingMaterialsController(ITrainingMaterialService trainingMaterialService, IFileProvider fileProvider)
        {
            _trainingMaterialService = trainingMaterialService;
            _fileProvider = fileProvider;
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> Download(Guid id)
        {
            var file = await _trainingMaterialService.GetFile(id);
            MemoryStream ms = new MemoryStream(file.TMatContent);

            byte[] content = ms.ToArray();
            string fileName = file.TMatName;
            string type = file.TMatType;                                                                                                            

            return File(content, _trainingMaterialService.GetMimeTypes()[type], fileName);
        }

        [HttpPost("UploadFileToDatabase")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingMaterialPermission), nameof(PermissionEnum.Create))]
        public async Task<IActionResult> Upload(IFormFile file, Guid lectureId)
        {
            await _trainingMaterialService.Upload(file, lectureId);
            if (file == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("UploadStaticFile")]
        public IActionResult UploadStaticFile(IFormFile file)
        {
            try
            {
                //var file = Request.Form.Files[0];

                var folderName = Path.Combine("wwwroot");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    DateTimeOffset lastModifiedDate = _fileProvider.GetFileInfo(dbPath).LastModified;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("DeleteFile")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingMaterialPermission), nameof(PermissionEnum.Modifed))]
        public async Task<IActionResult> DeleteTrainingMaterial(Guid id)
        {
            bool deleteTraingMaterial = await _trainingMaterialService.DeleteTrainingMaterial(id);
            if (deleteTraingMaterial)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("EditFile")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingMaterialPermission), nameof(PermissionEnum.Modifed))]
        public async Task<IActionResult> UpdateTrainingMaterial(IFormFile file, Guid id)
        {
            bool updateTrainingMaterial = await _trainingMaterialService.UpdateTrainingMaterial(file, id);
            if (updateTrainingMaterial)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
