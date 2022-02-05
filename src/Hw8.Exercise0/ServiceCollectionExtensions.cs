using Common;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace Hw8.Exercise0;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClientApplication(this IServiceCollection @this)
    {
        return @this
            // TODO: Add your other dependencies here
            .AddSingleton(sp => sp.GetRequiredService<MockHttpMessageHandler>().ToHttpClient())
            .AddTransient<HttpClientApplication>();
    }
}
