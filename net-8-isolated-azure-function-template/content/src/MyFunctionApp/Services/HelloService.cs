using MyFunctionApp.Models;

namespace MyFunctionApp.Services;

public interface IHelloService
{
    string SayHello(FormData formData);
}

public class HelloService : IHelloService
{
    public string SayHello(FormData formData)
    {
        if (formData.Gender == 'M')
        {
            return $"Hello, Mr {formData.Name} {formData.Surname}!";
        }          
        return $"Hello, Mrs {formData.Name} {formData.Surname}!";
    }
}