namespace ProjectOversight.API.Dto
{
    public class AssetCodeDTO
    {
        public int Id { get; set; }
        public int AssetCode { get; set; }
        public string? ProductId { get; set; }
        public string? AssetName { get; set; }
        public string? AssetDescription { get; set; }
        public bool? IsAllocated { get; set; }
        public string? AssetType { get; set; }
        public string? Brand { get; set; }
    }
}
