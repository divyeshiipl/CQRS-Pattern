namespace Application.Common;

public static class CommonUrlHelper
{
    public static string GetAbsoluteUrl(HttpRequest httpRequest)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = httpRequest.Scheme;
        uriBuilder.Host = httpRequest.Host.Host;
        uriBuilder.Path = httpRequest.PathBase + httpRequest.Path.ToString();
        uriBuilder.Query = httpRequest.QueryString.ToString();
        return uriBuilder.Uri.AbsoluteUri;
    }
}
