using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyFunctionApp.Services;

internal class Program
{
    private static void Main(string[] args)
    {
      var host = new HostBuilder()
      .ConfigureFunctionsWebApplication()
      .ConfigureServices(services =>{
        services.AddLogging();
        services.AddSingleton<IHelloService, HelloService>();
        })
      .Build();
      host.Run();
    }
}
