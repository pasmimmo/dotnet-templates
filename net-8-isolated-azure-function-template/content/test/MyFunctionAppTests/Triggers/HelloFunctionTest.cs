using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using MyFunctionApp.Models;
using MyFunctionApp.Services;
using MyFunctionApp.Triggers;
using MyFunctionAppTests.FakeData;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MyFunctionAppTests.Triggers
{
  [TestFixture]
  public class HelloFunctionTest
  {
    private Mock<ILogger<HelloTrigger>> loggerMock;
    private Mock<IHelloService> helloServiceMock;
    private Mock<FunctionContext> functionContextMock;


    private HelloTrigger helloTrigger;

    [SetUp]
    public void Setup()
    {
      loggerMock = new Mock<ILogger<HelloTrigger>>();
      helloServiceMock = new Mock<IHelloService>();
      helloTrigger = new HelloTrigger(helloServiceMock.Object, loggerMock.Object);
      functionContextMock = new Mock<FunctionContext>();
    }

    [Test]
    public async Task HelloApi_ValidData_ReturnsOk()
    {
      // Arrange
      var formData = new FormData { Name = "John", Surname = "Doe", Gender = 'M' };
      var requestBody = JsonConvert.SerializeObject(formData);
      var memEncode = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
      var requestData = new FakeHttpRequestData(functionContextMock.Object, new Uri("http://localhost:7044/api/hello"), memEncode);

      helloServiceMock.Setup(s => s.SayHello(It.IsAny<FormData>())).Returns("Hello, Mr John Doe!");

      // Act
      var response = await helloTrigger.HelloApi(requestData);

      // Assert
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
      //Assert.That(responseBody, Is.EqualTo("Hello, Mr John Doe!"));
    }

    [Test]
    public async Task HelloApi_InvalidData_ReturnsBadRequest()
    {

      var formData = "{}";
      var requestBody = JsonConvert.SerializeObject(formData);
      var memEncode = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
      var requestData = new FakeHttpRequestData(functionContextMock.Object, new Uri("http://localhost:7044/api/hello"), memEncode);

      // Act
      var response = await helloTrigger.HelloApi(requestData);

      // Assert
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
      var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
      //Assert.That(responseBody, Is.EqualTo("Wrong data received"));
    }

    [Test]
    public async Task HelloForm_ReturnsHtmlForm()
    {
      // Arrange
      var context = new Mock<FunctionContext>();
      var requestData = new FakeHttpRequestData(context.Object, new Uri("http://localhost:7044/api/hello"));

      // Act
      var response = await helloTrigger.HelloForm(requestData);

      // Assert
      Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      Assert.IsTrue(response.Headers.Contains("Content-Type"));
      Assert.That(response.Headers.GetValues("Content-Type").First(), Is.EqualTo("text/html; charset=utf-8"));
    }
  }
}
