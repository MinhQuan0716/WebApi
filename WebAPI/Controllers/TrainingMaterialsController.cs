using Application.Interfaces;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.FileProviders;
using System.Reflection.Metadata;
using Domain.Entities;
using Application.Services;

namespace WebAPI.Controllers
{
    public class TrainingMaterialsController : BaseController
    {
        private readonly ITrainingMaterialService _trainingMaterialService;

        /*[HttpPost("UploadFile")]
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
        }*/

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

        //===========================================
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public TrainingMaterialsController(ITrainingMaterialService trainingMaterialService)
        {
            // Replace the connection string and container name with your own values
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=storefilefatraining;AccountKey=V4fnfSO/yI5wyXQZNmUWNIdjcZhsI0C6Btwl7anlJbFBnPDmZa8q7vLv9PBvpf8ev0pqbhINBEHC+ASttAIwGw==;EndpointSuffix=core.windows.net";
            _containerName = "rootcontainer";
            // Create a BlobServiceClient object using the connection string
            _blobServiceClient = new BlobServiceClient(connectionString);
            _trainingMaterialService = trainingMaterialService;

        }

        [HttpPost]
        public async Task<IActionResult> UploadTestAzure(IFormFile file, Guid lectureId)
        {
            // Verify that the file was provided
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please provide a file");
            }

            // Get a reference to the container
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Create a unique name for the blob
            /*            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                        {
                            DateTimeOffset? dateTimeOffset = blobItem.Properties.LastModified;
                        }*/

            var blobName = Guid.NewGuid().ToString() + "." + file.FileName/* + Path.GetExtension(file.FileName)*/;
            //var blobName = "a5688551-bfdf-48e0-b440-e169d3f4321d.noelle.jpg";

            // Upload the file to the container
            var blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL of the uploaded blob
            var blobUrl = blobClient.Uri.AbsoluteUri;

            // Upload to database
            await _trainingMaterialService.Upload(file, lectureId, blobUrl);

            return Ok(blobUrl);
        }

        [HttpGet("{blobName}")]
        public async Task<IActionResult> DownloadTestAzure(string blobName)
        {
            // Get a reference to the container
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(blobName);

            // Download the blob to a stream
            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);

            // Get name without guid
            var result = blobName.Substring(blobName.IndexOf('.') + 1);

            // Reset the stream position to the beginning
            stream.Position = 0;

            // Get the properties of the blob
            var response = await blobClient.GetPropertiesAsync();

            // Return the file as a stream with the correct content type

            return File(stream, response.Value.ContentType, result);
        }

        [HttpPost]
        public async Task<IActionResult> EditTestAzure(string blobName, IFormFile file)
        {
            // Verify that the file was provided
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please provide a file");
            }

            if (blobName == null)
            {
                return BadRequest("Please provide a file to edit");
            }

            // Get a reference to the container
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Upload the file to the container
            var blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL of the uploaded blob
            var blobUrl = blobClient.Uri.AbsoluteUri;
            return Ok(blobUrl);
        }
    }
}