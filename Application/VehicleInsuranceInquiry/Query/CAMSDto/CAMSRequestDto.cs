namespace Application.VehicleInsuranceInquiry.Query.CAMSDto
{
    public class CamsRequestDto
    {
        public List<string> policyNumbers { get; set; } = new List<string>();
        public List<string> vehicleNumbers { get; set; } = new List<string>();
    }
}
