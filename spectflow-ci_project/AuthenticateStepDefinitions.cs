using RestSharp;
using TechTalk.SpecFlow;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MyNamespace
{
    [Binding]
    public class StepDefinitions
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        public StepDefinitions()
        {
            client = new RestClient();
        }

        [Given(@"I have the following credentials:")]
        public void GivenIHaveTheFollowingCredentials(Table table)
        {
            // Obtener los valores de la tabla
            var username = table.Rows[0]["username"];
            var password = table.Rows[0]["password"];
            var jsonBody = new
            {
                username = username,
                password = password
            };

            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(jsonBody);

            client = new RestClient("https://demostore.gatling.io/api");
            request = new RestRequest("authenticate", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
        }

        [When(@"I send a POST request to authenticate")]
        public void WhenISendAPOSTRequestTo()
        {
            response = client.Execute(request);
        }

        [Then(@"I expect to receive a valid response with status code OK in authenthication")]
        public void ThenIExpectToReceiveAValidResponseWithStatusCodeOK()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the response body contains an authentication token")]
        public void ThenTheResponseBodyContainsAnAuthenticationToken()
        {
            var content = response.Content;
            Assert.IsTrue(!string.IsNullOrEmpty(content));

            // Deserializar el contenido JSON y obtener el token
            JObject responseObject = JObject.Parse(content);
            string token = responseObject.GetValue("token").ToString();
            Console.WriteLine("Token: " + token);
        }
    }
}

