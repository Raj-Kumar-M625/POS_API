using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProjectOversight.API.Dto
{
    public class DocumentUploadDto
    {
        [Required]
        [JsonProperty("documentType")]
        public string DocumentType { get; set; } = null!;
        [Required][JsonProperty("taskid")] public int TaskId { get; set; }
        [Required][JsonProperty("fileName")] public string FileName { get; set; } = null!;
        [Required][JsonProperty("file")] public IFormFile File { get; set; } = null!;
    }
}
