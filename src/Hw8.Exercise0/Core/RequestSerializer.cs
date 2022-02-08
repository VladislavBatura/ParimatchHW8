namespace Hw8.Exercise0.Core;

public static class RequestSerializer
{
    public static bool SendRequestSerializeResult(HttpClient HttpClient,
        string RequestURL, string Cache,
        JsonHandler JsonHandler)
    {
        var st = RequestHandler.TryRequest(HttpClient, RequestURL);
        if (st.Length == 0)
        {
            return false;
        }
        else
        {
            if (!JsonHandler.Serialize(st, Cache))
            {
                return false;
            }
        }
        return true;
    }
}
