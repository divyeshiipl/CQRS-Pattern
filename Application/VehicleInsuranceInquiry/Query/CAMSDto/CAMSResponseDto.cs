namespace Application.VehicleInsuranceInquiry.Query.CAMSDto
{
    public class CamsResponseDto
    {
        public List<object>? errors { get; set; }
        public bool isValid { get; set; }
        public List<Value>? values { get; set; }
    }

    public class ClaimsRecord
    {
        public string ClaimNumber { get; set; }
        public string SubClaimNumber { get; set; }
        public string PolicyHolderId { get; set; }
        public decimal? PolicyHolderLiability { get; set; }
        public string PolicyNumber { get; set; }
        public string SubPolicyNumber { get; set; }
        public string InsuranceCompany { get; set; }
        public decimal? SettlementAmount { get; set; }
        public DateTime? SettlementDate { get; set; }
    }

    public class Value
    {
        public List<ClaimsRecord>? claimsRecords { get; set; }
    }
}
