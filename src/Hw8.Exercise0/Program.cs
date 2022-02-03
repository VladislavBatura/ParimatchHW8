using Common;
using Hw5.Exercise0;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

var sp = new ServiceCollection()
    .AddSingleton(_ =>
    {
        var httpClientMock = new MockHttpMessageHandler();
        using var fallbackHttpClient = new HttpClient();
        httpClientMock.Fallback.WithAny().Respond(fallbackHttpClient);
        return httpClientMock;
    })
    .AddTransient<IFileSystemProvider, FileSystemProvider>()
    .AddTransient<HttpClientApplication>()
    .BuildServiceProvider();

return (int)sp.GetRequiredService<HttpClientApplication>().Run(args);
