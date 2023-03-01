using Application.Interfaces;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TrainingMaterialsController : BaseController
    {
        private readonly ITrainingMaterialService _trainingMaterialService;

        public TrainingMaterialsController(ITrainingMaterialService trainingMaterialService)
        {
            _trainingMaterialService = trainingMaterialService;
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

        [HttpPost("UploadFile")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingMaterialPermission), nameof(PermissionEnum.Create))]
        public async Task<IActionResult> Upload(IFormFile file, string name)
        {
            await _trainingMaterialService.Upload(file, name);
            if (file == null)
            {
                return NotFound();
            }
            return Ok();
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

        [HttpPut("Edit file")]
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
