// This is a fake implementation of HttpRequestData that can be used in unit tests.

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace MyFunctionAppTests.FakeData;

[ExcludeFromCodeCoverage]
public class FakeHttpRequestData(FunctionContext functionContext, Uri url, Stream body = null) : HttpRequestData(functionContext)
{
  public override Stream Body { get; } = body ?? new MemoryStream();
  public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();
  public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
  public override Uri Url { get; } = url;
  public override IEnumerable<ClaimsIdentity> Identities { get; }
  public override string Method { get; }
  public override HttpResponseData CreateResponse()
  {
    return new FakeHttpResponseData(FunctionContext);
  }
}
