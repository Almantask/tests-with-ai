using Microsoft.Extensions.Configuration;
using System.Net;

namespace Acceptance.StepDefinitions
{
    [Binding]
    public sealed class HealthCheckSteps
    {
        private static readonly HttpClient _client = new HttpClient();
        private HttpResponseMessage _response;
        private readonly IConfiguration _configuration;

        public HealthCheckSteps()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        [When(@"I send a GET request to (.*)")]
        public async Task WhenISendAGETRequestTo(string endpoint)
        {
            var baseUrl = _configuration["baseUrl"];
            var fullUrl = $"{baseUrl}{endpoint}";
            _response = await _client.GetAsync(fullUrl);
        }

        [Then(@"the response should be (.*)")]
        public void ThenTheResponseShouldBe(int expectedStatusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
        }
    }
}
