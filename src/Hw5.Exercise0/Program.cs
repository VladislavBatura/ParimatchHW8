using Common;
using Hw5.Exercise0;
using RichardSzalay.MockHttp;

var httpClientMock = new MockHttpMessageHandler();
httpClientMock.Fallback.WithAny().Respond(new HttpClient());
var app = new HttpClientApplication(httpClientMock, new FileSystemProvider());

return (int)app.Run(args);
