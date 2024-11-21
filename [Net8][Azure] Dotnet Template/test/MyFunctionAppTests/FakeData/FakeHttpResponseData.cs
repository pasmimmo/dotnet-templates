using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace MyFunctionAppTests.FakeData;

[ExcludeFromCodeCoverage]

public class FakeHttpResponseData(FunctionContext functionContext) : HttpResponseData(functionContext)
{
  private readonly FunctionContext functionContext = functionContext;

  public override HttpStatusCode StatusCode { get; set; }
  public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();
  public override Stream Body { get; set; } = new MemoryStream();
  public override HttpCookies Cookies { get; }

}
