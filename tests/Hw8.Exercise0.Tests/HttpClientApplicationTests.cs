using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Hw8.Exercise0.Tests;

public class HttpClientApplicationTests
{
    private const string CacheFile = "cache.json";
    private const string ApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
    private static readonly string _today = DateTime.Today.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    private static readonly string _yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1)).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

    private readonly object _usd = new
    {
        cc = "USD",
        rate = 27.5093,
        exchangedate = _today
    };

    private readonly object _eur = new
    {
        cc = "EUR",
        rate = 31.1722,
        exchangedate = _today
    };

    private readonly object _staleUsd = new
    {
        cc = "USD",
        rate = 27.5093,
        exchangedate = _yesterday
    };

    private readonly object _staleEur = new
    {
        cc = "EUR",
        rate = 31.1722,
        exchangedate = _yesterday
    };

    [Fact]
    public void AddHttpClientApplication_WhenResolve_ExpectedNoIFsAndHttpClientRegistration()
    {
        using var sp = CreateServiceProvider(_ => { });
        Action fsResolving = () => sp.GetRequiredService<IFileSystemProvider>();
        fsResolving.Should().Throw<InvalidOperationException>();
        Action httpClientResolving = () => sp.GetRequiredService<HttpClient>();
        httpClientResolving.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Run_WhenStaleCacheDataAndNbuAvailable_ExpectedNbuApiCallAndReturnsSuccess()
    {
        // arrange
        using var sp = CreateServiceProvider(x => x
            .AddSingleton(_ =>
            {
                var fileSystemProvider = Substitute.For<IFileSystemProvider>();
                fileSystemProvider.Exists(Arg.Is(CacheFile)).Returns(true);
                fileSystemProvider.Read(Arg.Is(CacheFile)).Returns(ToUtf8ByteStream(_staleUsd, _staleEur));
                return fileSystemProvider;
            })
            .AddSingleton(_ =>
            {
                var httpClientMock = new MockHttpMessageHandler();
                httpClientMock
                    .Expect(HttpMethod.Get, ApiUrl)
                    .Respond(HttpStatusCode.OK, "application/json", ToUtf8ByteStream(_usd, _eur));
                return httpClientMock;
            }));

        // act
        var exitCode = sp.GetRequiredService<HttpClientApplication>().Run("usd", "uah", "10");

        // assert
        exitCode.Should().Be(ReturnCode.Success);
        sp.GetRequiredService<IFileSystemProvider>().Received().Write(Arg.Is(CacheFile), Arg.Any<Stream>());
        sp.GetRequiredService<MockHttpMessageHandler>().VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Run_WhenCacheDataAndNbuAvailable_ExpectedReturnsSuccess()
    {
        // arrange
        using var sp = CreateServiceProvider(x => x
            .AddSingleton(_ =>
            {
                var fileSystemProvider = Substitute.For<IFileSystemProvider>();
                fileSystemProvider.Exists(Arg.Is(CacheFile)).Returns(true);
                fileSystemProvider.Read(Arg.Is(CacheFile)).Returns(ToUtf8ByteStream(_usd, _eur));
                return fileSystemProvider;
            })
            .AddSingleton<MockHttpMessageHandler>());

        // act
        var exitCode = sp.GetRequiredService<HttpClientApplication>().Run("usd", "uah", "10");

        // assert
        exitCode.Should().Be(ReturnCode.Success);
        sp.GetRequiredService<MockHttpMessageHandler>().VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Run_WhenNoCacheDataAndNbuAvailable_ExpectedWriteToCacheAndReturnsSuccess()
    {
        // arrange
        using var sp = CreateServiceProvider(x => x
            .AddSingleton(_ =>
            {
                var fileSystemProvider = Substitute.For<IFileSystemProvider>();
                fileSystemProvider.Exists(Arg.Is(CacheFile)).Returns(false);
                return fileSystemProvider;
            })
            .AddSingleton(_ =>
            {
                var httpClientMock = new MockHttpMessageHandler();
                httpClientMock
                    .Expect(HttpMethod.Get, ApiUrl)
                    .Respond(HttpStatusCode.OK, "application/json", ToUtf8ByteStream(_usd, _eur));
                return httpClientMock;
            }));

        // act
        var exitCode = sp.GetRequiredService<HttpClientApplication>().Run("usd", "uah", "10");

        // assert
        exitCode.Should().Be(ReturnCode.Success);
        sp.GetRequiredService<IFileSystemProvider>().Received().Write(Arg.Is(CacheFile), Arg.Any<Stream>());
        sp.GetRequiredService<MockHttpMessageHandler>().VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData("usd", "uah")]
    [InlineData("usdt", "uah", "10")]
    [InlineData("xxx", "uah", "10")]
    [InlineData("dfe", "wf8", "10")]
    [InlineData("usd", "yyy", "10")]
    [InlineData("usd", "uaht", "10")]
    [InlineData("usd", "uah", "ten")]
    [InlineData("usd", "uah", "10", "some arg")]
    public void Run_WhenNotValidArgs_ReturnsInvalidArgsExitCode(params string[] args)
    {
        // arrange
        using var sp = CreateServiceProvider(x => x
            .AddSingleton(_ => GetFilesProvider(new { }))
            .AddSingleton<MockHttpMessageHandler>());

        // act
        var exitCode = sp.GetRequiredService<HttpClientApplication>().Run(args);

        // assert
        exitCode.Should().Be(ReturnCode.InvalidArgs);
    }

    private static Stream ToUtf8ByteStream<T>(params T[] content)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content)));
    }

    private static ServiceProvider CreateServiceProvider(Action<IServiceCollection> configure)
    {
        var sc = new ServiceCollection().AddHttpClientApplication();
        configure(sc);

        return sc.BuildServiceProvider();
    }

    private static IFileSystemProvider GetFilesProvider<T>(params T[] content)
    {
        var filesProvider = Substitute.For<IFileSystemProvider>();
        filesProvider
            .Read(Arg.Is(CacheFile))
            .Returns(ToUtf8ByteStream(content));

        return filesProvider;
    }
}
