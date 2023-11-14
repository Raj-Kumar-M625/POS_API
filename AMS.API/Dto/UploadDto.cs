using EllipticCurve.Utils;

namespace ProjectOversight.API.Dto
{
    public class UploadDto
    {
        public int TaskId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool DocumentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string? Images { get; set; }
       
        

       

        



    }
}
