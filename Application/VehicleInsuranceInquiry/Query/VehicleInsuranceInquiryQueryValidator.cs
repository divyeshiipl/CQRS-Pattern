namespace Application.VehicleInsuranceInquiry.Query;

public class VehicleInsuranceInquiryQueryValidator : AbstractValidator<VehicleInsuranceInquiryQuery>
{
    public VehicleInsuranceInquiryQueryValidator()
    {

        RuleFor(v => v.VehicleSearchType)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(string.Format(AppResource.REQUIRED, "{PropertyName}"))
            .Must(value => BeAValidInteger(value)).WithMessage(string.Format(AppResource.MUST_BE_INT, "{PropertyName}"))
            .Must(enumValue => EnumHelper.IsEnumIdExists(enumValue, typeof(VehicleSearchType)))
            .WithMessage(string.Format(AppResource.ENUM_NOT_EXISTS, "{PropertyName}"));

       RuleFor(v => v.VehicleSearchValue)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage(string.Format(AppResource.REQUIRED, "{PropertyName}"))
        .Must((model, vehicleSearchValue) =>
        {
            if (string.IsNullOrEmpty(vehicleSearchValue))
                return false;

            if (!int.TryParse(model.VehicleSearchType?.ToString(), out int typeValue) ||
                !Enum.IsDefined(typeof(VehicleSearchType), typeValue))
                return true;

            var vehicleSearchType = (VehicleSearchType)typeValue;

            return vehicleSearchType switch
            {
                VehicleSearchType.CustomId => vehicleSearchValue.Length <= 15,
                VehicleSearchType.VehicleSerialNumber => vehicleSearchValue.Length <= 15,
                VehicleSearchType.VehiclePlateNumber => vehicleSearchValue.Length <= 15,
                _ => true
            };
        })
        .WithMessage(model =>
        {
            if (!int.TryParse(model.VehicleSearchType?.ToString(), out int typeValue) ||
                !Enum.IsDefined(typeof(VehicleSearchType), typeValue))
                return string.Empty; 

            var vehicleSearchType = (VehicleSearchType)typeValue;

            return vehicleSearchType switch
            {
                VehicleSearchType.CustomId =>
                    string.Format(AppResource.LENGTH, nameof(model.VehicleSearchValue), 15),
                VehicleSearchType.VehicleSerialNumber =>
                    string.Format(AppResource.LENGTH, nameof(model.VehicleSearchValue), 15),
                VehicleSearchType.VehiclePlateNumber =>
                    string.Format(AppResource.LENGTH, nameof(model.VehicleSearchValue), 15),
                _ => string.Empty
            };
        });
    }



    private static bool BeAValidInteger(string property)
    {
        if (!string.IsNullOrEmpty(property))
            return int.TryParse(property, out _);
        return true;
    }
}
