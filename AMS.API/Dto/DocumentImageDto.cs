using Newtonsoft.Json;

namespace ProjectOversight.API.Dto
{
    public class DocumentImageDto
    {
        [JsonProperty("fileName")] public string FileName { get; set; } = null!;

        [JsonProperty("employeeid")] public int EmployeeId { get; set; }
        [JsonProperty("file")] public IFormFile File { get; set; } = null!;
    }
}
