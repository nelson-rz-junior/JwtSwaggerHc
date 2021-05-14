using APIClientV1;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;

namespace JwtSwaggerHc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // npm install nswag -g (install nswag command line tool)

            // Run the following commands:

            // cd JwtSwaggerHc.Client
            // install-package Newtonsoft.Json -version 12.0.1
            // nswag openapi2csclient /input:https://localhost:44352/swagger/v1/swagger.json /classname:SwaggerClientV1 /namespace:APIClientV1 /output:OpenAPIs/SwaggerClientV1.cs /runtime:Net50
            // nswag openapi2csclient /input:https://localhost:44352/swagger/v2/swagger.json /classname:SwaggerClientV2 /namespace:APIClientV2 /output:OpenAPIs/SwaggerClientV2.cs /runtime:Net50

            var baseUrl = "https://localhost:44352";
            var loginRequest = new LoginRequest { Username = "admin1", Password = "1234" };

            var clientV1 = new SwaggerClientV1(baseUrl, new HttpClient());
            var responseV1 = await clientV1.LoginAsync("", loginRequest);

            WriteLine($"Token = {responseV1.Token}");

            WriteLine();

            var clientV2 = new SwaggerClientV1(baseUrl, new HttpClient());
            var responseV2 = await clientV2.LoginAsync("", loginRequest);

            WriteLine($"Token = {responseV2.Token}");

            ReadKey();
        }
    }
}
