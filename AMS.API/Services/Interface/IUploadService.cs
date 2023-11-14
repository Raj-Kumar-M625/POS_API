using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Dto;

namespace ProjectOversight.API.Services.Interface
{
    public interface IUploadService
    {
        Task<Upload> UploadDocument(DocumentUploadDto uploadDto);

        Task<UploadDto>GetDocument(int TaskId);

    }
}
