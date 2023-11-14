namespace ProjectOversight.API.Dto
{
    public class AssetDTO
    {
        public int Id { get; set; }
        public int EmpCodeTblId { get; set; }
        public int AssetCodeTblId { get; set; }
        public string? EmpCode { get; set; }
        public int AssetCode { get; set; }
        public string? Remarks { get; set; }
        public DateTime? HandoverDate { get; set; }
        public string? HandoverBy { get; set; }
        public string? ReceivedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ReasonForAsset { get; set; }
        public bool? IsAllocated { get; set; }
        public string? AssetType { get; set; }
    }
}
