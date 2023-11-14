using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOversight.API.Data.Model
{
    public class Document:BaseEntity
    {
        public string TableName { get; set; }
        public int AttributeId { get; set; }
        public int ProjectId { get; set; }
        public string DocType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string? FilePath { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        [NotMapped]
        public byte[]? FileContent { get; set; }
    }
}
