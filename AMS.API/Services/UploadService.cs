using AutoMapper;
using ProjectOversight.API.Data;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
using ProjectOversight.API.Dto;
using ProjectOversight.API.Services.Interface;
using Task = ProjectOversight.API.Data.Model.Task;

namespace ProjectOversight.API.Services
{
    public class UploadService : IUploadService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly ProjectOversightContext _dbContext;

        public UploadService(
            IMapper mapper, IConfiguration configuration, IUnitOfWork repository, ProjectOversightContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _repository = repository;
            _dbContext = context;
        }

        public async Task<UploadDto> GetDocument(int TaskId)
        {
            try
            {
                string imagesFolder = _configuration["ImagePathSetting:AllowPath"];
                var image = await _repository.Upload.FindByConditionAsync(x => x.TaskId == TaskId && x.DocumentType == "image/jpeg");
                var upload = image.FirstOrDefault();
                if (upload == null)
                {
                    return null;
                }

                var FilePath = Path.Combine(imagesFolder, upload.TaskId.ToString(), upload.FileName);
                var folder = File.ReadAllBytes(FilePath);
                UploadDto result = new UploadDto();
                result.Images = Convert.ToBase64String(folder);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed.
                // You might also consider rethrowing the exception or returning an error response.
                throw;
            }
        }


        public async Task<Upload> UploadDocument(DocumentUploadDto uploadDto)
        {
            try
            {
                var task = await _repository.Task.FindById(x =>x.Id == uploadDto.TaskId);
                var formFile = uploadDto.File;
                var projectPath = _configuration.GetValue<string>("DocumentFilePath");
                var folderPath = Path.Combine(projectPath, task.Id.ToString());
                var filePath = Path.Combine(folderPath, formFile.FileName);
                var fileName = Path.GetFileName(formFile.FileName);


                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
                fileStream.Flush();
                fileStream.Dispose();
                var ImageConvertToByte = File.ReadAllBytes(filePath);
                var base64String = Convert.ToBase64String(ImageConvertToByte);
                Upload upload = new()
                {
                    DocumentType = uploadDto.DocumentType,
                    FilePath = filePath,
                    DocumentStatus = false,
                    FileName = fileName,
                    TaskId = uploadDto.TaskId,
                };

                var response = await _repository.Upload.CreateAsync(upload);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
    
}
