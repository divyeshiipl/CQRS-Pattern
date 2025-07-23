namespace Application.Common.Mappings
{
    public class VehicleInsuranceInquiryMapper : Profile
    {
        public VehicleInsuranceInquiryMapper()
        {
            CreateMap<Policy, VehicleInsuranceInquiryDetailsResponseDto>()
                .ForMember(dest => dest.main_insurance_number, opt => opt.MapFrom(src => src.policyNumber))
                .ForMember(dest => dest.insurance_company_code, opt => opt.MapFrom(src => src.insuranceCompanyCode))
                .ForMember(dest => dest.insurance_company_name_arabic, opt => opt.MapFrom(src => src.insuranceCompanyNameArabic))
                .ForMember(dest => dest.insurance_company_name_english, opt => opt.MapFrom(src => src.insuranceCompanyNameEnglish))
                .ForMember(dest => dest.insurance_coverage_type_code, opt => opt.MapFrom(src => src.coverageCode))
                .ForMember(dest => dest.insurance_coverage_type_description_arabic, opt => opt.MapFrom(src => src.coverageTypeArabic))
                .ForMember(dest => dest.insurance_coverage_type_description_english, opt => opt.MapFrom(src => src.coverageTypeEnglish))
                .ForMember(dest => dest.insurance_issue_date, opt => opt.MapFrom(src => src.issueGreDate))
                .ForMember(dest => dest.insurance_start_date, opt => opt.MapFrom(src => src.effectiveGreDate))
                .ForMember(dest => dest.insurance_expiry_date, opt => opt.MapFrom(src => src.expireGreDate))
                .ForMember(dest => dest.policy_id, opt => opt.MapFrom(src => src.policyId))
                .ForMember(dest => dest.insurance_status_code, opt => opt.MapFrom(src => src.insuranceStatusCode))
                .ForMember(dest => dest.insurance_status_description_arabic, opt => opt.MapFrom(src => src.insuranceStatusDescriptionArabic))
                .ForMember(dest => dest.insurance_status_description_english, opt => opt.MapFrom(src => src.insuranceStatusDescriptionEnglish))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.id_type, opt => opt.MapFrom(src => src.idType))
                .ForMember(dest => dest.id_number, opt => opt.MapFrom(src => src.idNumber))
                .ForMember(dest => dest.mobile_number, opt => opt.MapFrom(src => src.mobileNumber))
                .ForMember(dest => dest.national_address, opt => opt.MapFrom(src => src.nationalAddress))
                .ForMember(dest => dest.owner_name, opt => opt.MapFrom(src => src.ownerName));

            CreateMap<vehicle, VehicleResponseDto>()
                .ForMember(dest => dest.serial_code, opt => opt.MapFrom(src => src.serialCode))
                .ForMember(dest => dest.vehicle_color_arabic, opt => opt.MapFrom(src => src.vehicleColorArabic))
                .ForMember(dest => dest.vehicle_color_english, opt => opt.MapFrom(src => src.vehicleColorEnglish))
                .ForMember(dest => dest.vehicle_type_arabic, opt => opt.MapFrom(src => src.vehicleTypeArabic))
                .ForMember(dest => dest.vehicle_type_english, opt => opt.MapFrom(src => src.vehicleTypeEnglish))
                .ForMember(dest => dest.vehicle_plate_arabic, opt => opt.MapFrom(src => src.vehiclePlateArabic))
                .ForMember(dest => dest.vehicle_plate_english, opt => opt.MapFrom(src => src.vehiclePlateEnglish))
                .ForMember(dest => dest.full_plate_number_arabic, opt => opt.MapFrom(src => src.fullPlateNumberArabic))
                .ForMember(dest => dest.full_plate_number_english, opt => opt.MapFrom(src => src.fullPlateNumberEnglish));


            CreateMap<VehicleInsuranceInquiryAPIGEEResponseDto, VehicleInsuranceInquiryResponseDto>()
                .ForMember(dest => dest.number_of_results_found, opt => opt.MapFrom(src => src.totalCount))
                .ForMember(dest => dest.policies, opt => opt.MapFrom(src => src.policies));
        }
    }
}
