namespace Application.Interfaces;

public interface ILogService
{
    void LogError(Exception ex);
    void LogAudit(string requestBody,
        string responseBodyText, string queryParameters, DateTime createdOn, DateTime updatedOn);
    void LogInfo(string message);

}