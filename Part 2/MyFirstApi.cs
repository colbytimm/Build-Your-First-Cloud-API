using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class MyFirstApi
    {
        [Function("Multiply")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Multiply");
            logger.LogInformation("Running the multiply API");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            try
            {
                string requestBody = String.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    requestBody = await streamReader.ReadToEndAsync();
                }
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                double x = data?.x;
                double y = data?.y;
                logger.LogInformation($"Multiplying {x} and {y}");

                double result = x * y;

                response.WriteString($"Result: {result}");
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred when trying to multiply your two numbers: {ex.Message}");
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Failed to multiply");
            }

            return response;
        }

    }
}
