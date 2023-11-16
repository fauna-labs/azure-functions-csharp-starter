using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Example.Functions
{
  public class FaunaExample
  {
    private readonly ILogger _logger;
    private readonly HttpClient _http;

    public FaunaExample(ILoggerFactory loggerFactory, HttpClient httpClient)
    {
      _logger = loggerFactory.CreateLogger<FaunaExample>();
      _http = httpClient;
      _http.BaseAddress = new Uri("https://db.fauna.com/");
      _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("FAUNA_KEY"));
    }


    [Function("FaunaExample")]
    public async Task<HttpResponseData> CallFauna([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
      [FromBody] Parameters input)
    {
      _logger.LogInformation("Request Params: {}", input);

      var inputFirstName = input.StringInput1;

      using StringContent requestJson = new(
          JsonSerializer.Serialize(new
          {
            query = "customers.where(.firstName == arg1)",
            arguments = new {
              arg1 = inputFirstName
            }
          }),
          Encoding.UTF8,
          "application/json");

      var faunaResponse = await _http.PostAsync("query/1", requestJson);
      var jsonResponse = await faunaResponse.Content.ReadAsStringAsync();

      var response = req.CreateResponse(HttpStatusCode.OK);
      response.Headers.Add("Content-Type", "application/json");
      await response.WriteStringAsync(jsonResponse);

      return response;
    }

    public record Parameters(string StringInput1, string StringInput2, int IntInput1, int IntInput2);
  }
}
