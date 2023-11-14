namespace ProjectOversight.API.Data.Model
{
    public class AppVersion
    {
        public int ID { get; set; }
        public string? VersionCode { get; set; }
        public DateTime? CreatedDate {get;set;}
        public DateTime? ExpiryDate { get;set;}
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

    }
}
