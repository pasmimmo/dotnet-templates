using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MyFunctionApp.Services;
using MyFunctionApp.Models;

namespace MyFunctionApp.Triggers;

public class HelloTrigger(IHelloService helloService, ILogger<HelloTrigger> logger)
{
    private readonly IHelloService helloService = helloService;
    private readonly ILogger<HelloTrigger> log = logger;

    [Function("SayHelloApi")]
    public async Task<HttpResponseData> HelloApi(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "hello")] HttpRequestData req)
    {
        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var formData = JsonConvert.DeserializeObject<FormData>(requestBody);
            CheckFormData(formData);
            var greetings = helloService.SayHello(formData);
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(greetings);
            return response;
        }
        catch (Exception e)
        {
            log.LogError(e, "Error while processing request");
            var response = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await response.WriteStringAsync("Wrong data received");
            return response;
        }
    }

    [Function("SayHelloForm")]
    public async Task<HttpResponseData> HelloForm(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "hello")] HttpRequestData req)
    {
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/html; charset=utf-8");

        // Costruisci il percorso relativo al file HTML
        var assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var filePath = Path.Combine(assemblyPath, "www", "form.html");

        string htmlForm = await File.ReadAllTextAsync(filePath);

        await response.WriteStringAsync(htmlForm);
        return response;
    }



    private void CheckFormData(FormData formData)
    {
        if (formData.Name == null || formData.Surname == null )
        {
            throw new ArgumentException("Name, Surname and Gender are required");
        }
    }
}
