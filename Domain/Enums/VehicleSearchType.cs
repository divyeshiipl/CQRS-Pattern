namespace Domain.Enums;

public enum VehicleSearchType
{
    [Description("Vehicle Plate Number")]
    VehiclePlateNumber = 1,

    [Description("Vehicle Serial Number")]
    VehicleSerialNumber = 2,

    [Description("Custom_Id")]
    CustomId = 3,
}