using System.Text;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using Xunit;

public class LoadTests
{
    [Fact]
    public void Register_LoadTest_500RPS()
    {
        var scenario = Scenario.Create("register_load", async context =>
        {
            using var client = new HttpClient();
            var email = $"user_{context.InvocationNumber}@test.com";

            var response = await client.PostAsync("https://localhost:5001/Account/Register",
                new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        username = $"user_{context.InvocationNumber}",
                        email = email,
                        password = "Password123!"
                    }),
                    Encoding.UTF8,
                    "application/json"
                ));

            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: 500,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromMinutes(2))
        );

        NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFileName("load_report")
            .Run();
    }
    [Fact]
    public void Register_InvalidData_LoadTest()
    {
        var scenario = Scenario.Create("register_invalid_load", async context =>
        {
            using var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:5001/Account/Register",
                new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        username = "", // Невалидное поле
                        email = "invalid-email",
                        password = "123"
                    }),
                    Encoding.UTF8,
                    "application/json"
                ));

            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: 100,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromMinutes(1))
        );

        NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFileName("invalid_load_report")
            .Run();
    }
}