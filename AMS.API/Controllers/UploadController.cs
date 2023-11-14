using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
  
    public class UploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        public UploadController(IUnitOfWork repository, IConfiguration configuration, IMapper mapper, IUploadService uploadService)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _uploadService = uploadService;
            
        }
        [HttpPost("UploadDocument")]
        [Authorize]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto uploadDto)
        {
            try
            {
               
                if (uploadDto.File == null) return new UnsupportedMediaTypeResult();
                {
                    var response = await _uploadService.UploadDocument(uploadDto);
                    return Ok(new { response.Id });
                }

            }
            catch (Exception ex)
            {
              
                throw;
            }

        }


        [HttpGet("GetDocument")]
        [Authorize]
        public async Task<IActionResult> GetDocument(int TaskId)
        {
            try
            {
                var response = await _uploadService.GetDocument(TaskId);
                return Ok(response);
              
            }
            catch (Exception ex)
            {

                throw;
            }

        }
     
        [HttpGet("GetAPK")]
        public async Task<IActionResult> DownloadAPK(string VersionCode)
        {
            string apkFolder = _configuration["APKPathSetting:AllowPath"];
            var appversion = await _repository.Appversion.FindByConditionAsync(x => x.VersionCode == VersionCode);
            var apk = appversion.FirstOrDefault();
            if (appversion == null)
            {
                return null!;
            }
            var FilePath = Path.Combine(apkFolder, apk.FileName);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(FilePath);
            var memoryStream = new MemoryStream(fileBytes);

            return File(memoryStream, "application/vnd.android.package-archive", apk.FileName);
        }

    }
}
