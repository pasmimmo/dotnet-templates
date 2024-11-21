using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MyFunctionApp.Trigger;

public class StaticFileServe(ILogger<StaticFileServe> logger)
{
  private readonly ILogger<StaticFileServe> _logger = logger;

  [Function("StaticFileServe")]
  public async Task<HttpResponseData> Run(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = "www/{fileName}")] HttpRequestData req,
      string fileName)
  {
    var response = req.CreateResponse(System.Net.HttpStatusCode.OK);

    // Costruisci il percorso relativo al file
    var assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    var filePath = Path.Combine(assemblyPath, "www", fileName);

    if (!File.Exists(filePath))
    {
      _logger.LogWarning($"File not found: {filePath}");
      response = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
      await response.WriteStringAsync("File not found");
      return response;
    }

    // Determina il tipo di contenuto in base all'estensione del file
    string contentType = fileName.EndsWith(".css") ? "text/css; charset=utf-8" :
                         fileName.EndsWith(".js") ? "application/javascript; charset=utf-8" :
                         "text/plain; charset=utf-8";

    response.Headers.Add("Content-Type", contentType);

    string fileContent = await File.ReadAllTextAsync(filePath);
    await response.WriteStringAsync(fileContent);
    return response;
  }
}