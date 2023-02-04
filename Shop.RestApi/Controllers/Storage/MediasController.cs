using Microsoft.AspNetCore.Mvc;
using MimeMapping;
using Shop.Infrastructures;
using Shop.Services.StorageServices.Contracts;
using Shop.Services.StorageServices.Contracts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Shop.RestApi.Controllers.Storage
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/medias")]
    public class MediasController : ControllerBase
    {
        private readonly StorageService _service;
        private readonly ImageService _imageService;

        public MediasController(
            StorageService service,
            ImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<string> Add([FromForm, Required] IFormFile file)
        {
            var documentDto = new AddMediaDto
            {
                Extension = Path.GetExtension(file.FileName),
                Data = await FormFileToByteArrayAsync(file)
            };
            return await _service.Add(documentDto);
        }

        [HttpGet("{id}")]
        public FileResult? Download(
            [FromRoute, Required] string id,
            [FromQuery] int? size)
        {
            var file = _service.GetById(id);

            if (file == null)
            {
                return null;
            }

            var data = file.Data;

            if (size.HasValue)
            {
                data = _imageService.GetThumbnail(file.Data, size.Value);
            }

            return File(data, MimeUtility.GetMimeMapping(file.Extension));
        }

        private async Task<byte[]> FormFileToByteArrayAsync(IFormFile file)
        {
            byte[] fileStream;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileStream = memoryStream.ToArray();
                await memoryStream.FlushAsync();
            }

            return fileStream;
        }
    }
}
