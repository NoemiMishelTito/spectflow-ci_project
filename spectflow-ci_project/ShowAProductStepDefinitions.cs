using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace spectflow_ci_project
{
    [Binding]
    public class ShowAProductStepDefinitions
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        [When(@"I send a GET request a product for ID (.*)")]
        public void WhenISendAGETRequestTo(int productId)
        {
            client = new RestClient("http://demostore.gatling.io/api");
            request = new RestRequest($"product/{productId}", Method.Get);
            response = client.Execute(request);
        }

        [Then(@"I expect to receive a valid response with status code OK productId")]
        public void ThenIExpectToReceiveAValidResponseWithStatusCodeOKProductId()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the response body contains the product details for ID (.*)")]
        public void ThenTheResponseBodyContainsTheProductDetailsForID(int productId)
        {
            var content = response.Content;
            Assert.IsTrue(!string.IsNullOrEmpty(content));

            try
            {
                JObject productObject = JObject.Parse(content);
                int id = productObject.GetValue("id").Value<int>();

                if (id == productId)
                {
                    string name = productObject.GetValue("name").Value<string>();
                    string price = productObject.GetValue("price").Value<string>();

                    Console.WriteLine($"Product ID: {id}");
                    Console.WriteLine($"Name: {name}");
                    Console.WriteLine($"Price: {price}");
                }
                else
                {
                    Assert.Fail($"Product with ID {productId} not found.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to parse response content as a product. Error: {ex.Message}");
            }
        }
    }
}
