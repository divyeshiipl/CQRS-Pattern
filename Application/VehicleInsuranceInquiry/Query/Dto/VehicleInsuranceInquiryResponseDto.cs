namespace Application.VehicleInsuranceInquiry.Query.Dto;

public class VehicleInsuranceInquiryResponseDto
{
    public int number_of_results_found { get; set; }
    public List<VehicleInsuranceInquiryDetailsResponseDto> policies { get; set; }
}

public class VehicleInsuranceInquiryDetailsResponseDto
{
    public string main_insurance_number { get; set; }
    public int insurance_company_code { get; set; }
    public string insurance_company_name_arabic { get; set; }
    public string insurance_company_name_english { get; set; }
    public int insurance_coverage_type_code { get; set; }
    public string insurance_coverage_type_description_arabic { get; set; }
    public string insurance_coverage_type_description_english { get; set; }
    public DateTime? insurance_issue_date { get; set; }
    public DateTime insurance_start_date { get; set; }
    public DateTime insurance_expiry_date { get; set; }
    public long policy_id { get; set; }
    public string insurance_status_code { get; set; }
    public string insurance_status_description_arabic { get; set; }
    public string insurance_status_description_english { get; set; }
    public string name { get; set; }
    public int id_type { get; set; }
    public long id_number { get; set; }
    public string mobile_number { get; set; }
    public string national_address { get; set; }
    public string owner_name { get; set; }
    public List<VehicleResponseDto> vehicles { get; set; }
}

public class VehicleResponseDto
{
    public long serial_code { get; set; }
    public string vehicle_color_arabic { get; set; }
    public string vehicle_color_english { get; set; }
    public string vehicle_type_arabic { get; set; }
    public string vehicle_type_english { get; set; }
    public string vehicle_plate_arabic { get; set; }
    public string vehicle_plate_english { get; set; }
    public string full_plate_number_arabic { get; set; }
    public string full_plate_number_english { get; set; }
}
