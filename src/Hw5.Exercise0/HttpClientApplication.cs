using Common;
using RichardSzalay.MockHttp;

namespace Hw5.Exercise0;

public class HttpClientApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly HttpClient _httpClient;

    public HttpClientApplication(MockHttpMessageHandler httpMessageHandler, IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
        _httpClient = httpMessageHandler.ToHttpClient();
    }

    /// <summary>
    /// Runs http client app.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>
    /// Returns <see cref="ReturnCode.Success"/> in case of successful exchange calculation.
    /// Returns <see cref="ReturnCode.InvalidArgs"/> in case of invalid <paramref name="args"/>.
    /// Returns <see cref="ReturnCode.Error"/> in case of error <paramref name="args"/>.
    /// </returns>
    public ReturnCode Run(params string[] args)
    {
        throw new NotImplementedException("Should be implemented by executor");
    }
}
