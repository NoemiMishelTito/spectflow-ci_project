using TechTalk.SpecFlow;
using RestSharp;
using Newtonsoft.Json;
using System.Net;

namespace spectflow_ci_project
{
    [Binding]
    public class UpdateAProductStepDefinitions
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        private string token;

        [Given(@"I am authenticated with a valid token")]
        public void GivenIAmAuthenticatedWithAValidToken()
        {
            client = new RestClient("https://demostore.gatling.io/api");
            request = new RestRequest("authenticate", Method.Post);
            var credentials = new { username = "admin", password = "admin" };
            request.AddJsonBody(credentials);
            response = client.Execute(request);

            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            token = tokenResponse.token;
        }
        [When(@"I send a PUT request for product for ID (.*)")]
        public void WhenISendAPUTRequestTo(int productId)
        {
            // Required
            var updatedProduct = new
            {
                name = "Black and Red Glasses",
                slug = "black-and-red-glasses",
                description = "<p>A Black &amp; Red glasses case</p>",
                image = "casual-blackred-open.jpg",
                price = "18.99",
                categoryId = "5"
            };

            client = new RestClient("http://demostore.gatling.io/api");
            request = new RestRequest($"product/{productId}", Method.Put);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(updatedProduct);

            response = client.Execute(request);
        }

        [Then(@"I expect to receive a valid response with status code OK after updated")]
        public void ThenIExpectToReceiveAValidResponseWithStatusCodeOKAfterUpdated()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the product with ID (.*) has been successfully updated")]
        public void ThenTheProductWithIDHasBeenSuccessfullyUpdatedWithTheNewPrice(int productId)
        {
            request = new RestRequest($"product/{productId}", Method.Get);
            response = client.Execute(request);
            var updatedProduct = JsonConvert.DeserializeObject<dynamic>(response.Content);
            Console.WriteLine($"ID: {updatedProduct.id}");
            Console.WriteLine($"Name: {updatedProduct.name}");
            Console.WriteLine($"Price: {updatedProduct.price}");
            Console.WriteLine($"Created At: {updatedProduct.createdAt}");
            Assert.AreEqual("18.99", updatedProduct.price.ToString());
        }
    }
}
