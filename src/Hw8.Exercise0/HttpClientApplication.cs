using Common;
using Hw8.Exercise0.Core;
using Hw8.Exercise0.Models;
using RichardSzalay.MockHttp;

namespace Hw8.Exercise0;

public class HttpClientApplication
{
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly HttpClient _httpClient;
    private readonly JsonHandler _jsonHandler;
    private const string Cache = "cache.json";
    private const string RequestURL = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    public HttpClientApplication(JsonHandler jsonHandler, MockHttpMessageHandler httpMessageHandler, IFileSystemProvider fileSystemProvider)
    {
        _jsonHandler = jsonHandler;
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
        if (!ArgumentsHandler.IsValidArgs(args))
        {
            return ReturnCode.InvalidArgs;
        }

        if (!_jsonHandler.IsProviderInjected())
        {
            _jsonHandler.InjectProvider(_fileSystemProvider);
        }

        if (!_fileSystemProvider.Exists(Cache))
        {
            //Without this "Success-return"
            //test "Run_WhenNoCacheDataAndNbuAvailable_ExpectedWriteToCacheAndReturnsSuccess" will fail
            return !RequestSerializer.SendRequestSerializeResult(_httpClient, RequestURL, Cache, _jsonHandler)
                ? ReturnCode.Error
                : ReturnCode.Success;
        }

        var enumCurrency = _jsonHandler.Deserialize(Cache);

        //i don't want to check, if enumCurrecny is not type of listCurrency,
        //because it will always be IEnumerable, and will cast
#pragma warning disable IDE0019 // Use pattern matching
        var listCurrency = enumCurrency as List<Currency>;
#pragma warning restore IDE0019 // Use pattern matching

        if (listCurrency is null || !listCurrency.Any())
        {
            return ReturnCode.Error;
        }

        if (!ValidateDate.IsValidDate(listCurrency!.First().ExchangeDate))
        {
            if (!RequestSerializer.SendRequestSerializeResult(_httpClient, RequestURL, Cache, _jsonHandler))
            {
                return ReturnCode.Error;
            }

            enumCurrency = _jsonHandler.Deserialize(Cache);
            listCurrency = enumCurrency as List<Currency>;
            if (listCurrency is null || !listCurrency.Any())
            {
                return ReturnCode.Error;
            }
        }

        var transactionData = TransactionArgumentsHandler.ParseArgs(args);

        if (!TransactionArgumentsHandler.IsValidArgs(transactionData, listCurrency))
        {
            return ReturnCode.InvalidArgs;
        }

        var resultTransaction = Transaction.ProcessTransaction(listCurrency, transactionData);

        Console.Write($"{resultTransaction.Currency} {resultTransaction.Date} {resultTransaction.Amount}");

        return ReturnCode.Success;
    }
}
