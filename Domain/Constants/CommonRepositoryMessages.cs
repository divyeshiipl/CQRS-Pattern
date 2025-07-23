namespace Domain.Constants;

public static class CommonRepositoryMessages
{
    public const string CommorErrorMsgEN = "Something went wrong.";
    public const string CommorErrorMsgAR = "";

    #region Response Message
    public const string InternalServerErrorTitle = "Internal Server Error";
    public const string InternalServerErrorDetails = "An unexpected error occurred. Please try again later.";

    public const string ValidationErrorTitle = "One or more validation failures have occurred";
    public const string ValidationDetails = "One or more validation failures have occurred.";

    public const string UnauthorizedAccessTitle = "Unauthorized";
    public const string UnauthorizedAccessDetails = "Unauthorized to access the path or record or info.";

    public const string ForbiddenAccessTitle = "Forbidden";
    public const string ForbiddenAccessDetails = "Forbidden to access the path";

    #endregion

    #region Success Message
    public const string SavedSuccessfullyTitle = "Saved Successfully";
    public const string SavedSuccessfullyDetail = "{0} details saved successfully.";

    public const string GeneratedSuccessfullyTitle = "Generated Successfully";
    public const string GeneratedSuccessfullyDetail = "{0} details generated successfully.";

    public const string UpdatedSuccessfullyTitle = "Updated Successfully";
    public const string UpdatedSuccessfullyDetail = "{0} details updated successfully.";
    #endregion
}
