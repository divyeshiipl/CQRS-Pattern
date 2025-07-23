namespace Application.VehicleInsuranceInquiry.Query.Dto
{
    public class VehicleInsuranceInquiryAPIGEEResponseDto
    {
        public int totalCount { get; set; }
        public List<Policy> policies { get; set; }
    }
    public class Policy
    {
        public string policyNumber { get; set; }
        public int? insuranceCompanyCode { get; set; }
        public string insuranceCompanyNameArabic { get; set; }
        public string insuranceCompanyNameEnglish { get; set; }
        public DateTime issueGreDate { get; set; }
        public DateTime effectiveGreDate { get; set; }
        public DateTime expireGreDate { get; set; }
        public int coverageCode { get; set; }
        public string coverageTypeArabic { get; set; }
        public string coverageTypeEnglish { get; set; }
        public long policyId { get; set; }
        public string insuranceStatusCode { get; set; }
        public string insuranceStatusDescriptionArabic { get; set; }
        public string insuranceStatusDescriptionEnglish { get; set; }
        public string name { get; set; }
        public int idType { get; set; }
        public long idNumber { get; set; }
        public string mobileNumber { get; set; }
        public string nationalAddress { get; set; }
        public string ownerName { get; set; }
        public List<vehicle> vehicles { get; set; }
    }

    public class vehicle
    {
        public long serialCode { get; set; }
        public string vehicleColorArabic { get; set; }
        public string vehicleColorEnglish { get; set; }
        public string vehicleTypeArabic { get; set; }
        public string vehicleTypeEnglish { get; set; }
        public string vehiclePlateArabic { get; set; }
        public string vehiclePlateEnglish { get; set; }
        public string fullPlateNumberArabic { get; set; }
        public string fullPlateNumberEnglish { get; set; }
    }
}
