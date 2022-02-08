namespace Hw8.Exercise0.Core;

public static class RequestHandler
{
    //if simplified, it will become less readable
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public static Stream TryRequest(HttpClient httpClient, string requestURL)
    {
        var result = httpClient.GetAsync(requestURL).Result;

        if (!result.IsSuccessStatusCode)
            return new MemoryStream();

        return result.Content.ReadAsStreamAsync().Result;
    }
}
