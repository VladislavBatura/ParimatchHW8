using System.Text.Json;
using Common;
using Hw8.Exercise0.Core.Interface;
using Hw8.Exercise0.Models;

namespace Hw8.Exercise0.Core;

public class JsonHandler : IJsonHandler
{
    private IFileSystemProvider? Provider { get; set; }

    public JsonHandler(IFileSystemProvider provider)
    {
        Provider = provider;
    }

    public bool IsProviderInjected()
    {
        return Provider != null;
    }

    public void InjectProvider(IFileSystemProvider provider)
    {
        Provider = provider;
    }

    public IEnumerable<object> Deserialize(string fileName)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            //Without seeking the beginning of the stream,
            //test "Run_WhenStaleCacheDataAndNbuAvailable_ExpectedNbuApiCallAndReturnsSuccess" will fail
            //Also, it will fail even if we dispose of this stream
            var stream = Provider!.Read(fileName);
            _ = stream.Seek(0, SeekOrigin.Begin);
            var listCurrency = JsonSerializer.Deserialize<List<Currency>>(stream, options);
            return listCurrency is null ? Enumerable.Empty<Currency>() : listCurrency;
        }
        catch
        {
            return Enumerable.Empty<Currency>();
        }
    }

    public bool Serialize(Stream content, string fileName)
    {
        try
        {
            _ = Provider!.WriteAsync(fileName, content);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
