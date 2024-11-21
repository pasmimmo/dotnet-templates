using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MyFunctionApp.Trigger;
using MyFunctionAppTests.FakeData;
using System.Net;

namespace MyFunctionAppTests.Triggers;

[TestFixture]
public class StaticFileServeTest
{
  private Mock<ILogger<StaticFileServe>> loggerMock;
  private StaticFileServe staticFileServe;
  private HttpRequestData requestData;

  [SetUp]
  public void Setup()
  {
    loggerMock = new Mock<ILogger<StaticFileServe>>();
    staticFileServe = new StaticFileServe(loggerMock.Object);
    var context = new Mock<FunctionContext>();

    requestData = new FakeHttpRequestData(context.Object, new Uri("http://localhost:7044/api/www"));
  }

  [Test]
  public async Task Run_FileExists_ReturnsFileContent()
  {
    var x = await staticFileServe.Run(requestData, "file.notFound");
    Assert.That(x.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
  }
}
