using MyFunctionApp.Models;
using MyFunctionApp.Services;

namespace MyFunctionAppTests.Services;

[TestFixture]
public class HelloServiceTest
{
  private IHelloService _helloService;

  [SetUp]
  public void Setup()
  {
    _helloService = new HelloService();
  }

  [Test]
  public void SayHello_WhenSessoIsM_ReturnsHelloMr()
  {
    // Arrange
    var formData = new FormData
    {
      Name = "John",
      Surname = "Doe",
      Gender = 'M'
    };

    // Act
    var result = _helloService.SayHello(formData);

    // Assert
    Assert.That(result, Is.EqualTo("Hello, Mr John Doe!"));
  }

  [Test]
  public void SayHello_WhenSessoIsNotM_ReturnsHelloMrs()
  {
    // Arrange
    var formData = new FormData
    {
      Name = "Jane",
      Surname = "Doe",
      Gender = 'F'
    };

    // Act
    var result = _helloService.SayHello(formData);

    // Assert
    Assert.That(result, Is.EqualTo("Hello, Mrs Jane Doe!"));
  }

}
