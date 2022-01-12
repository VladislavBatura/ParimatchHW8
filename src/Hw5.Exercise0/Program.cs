using Common;
using Hw5.Exercise0;
using RichardSzalay.MockHttp;

var app = new HttpClientApplication(new MockHttpMessageHandler(), new FileSystemProvider());

return (int)app.Run(args);
