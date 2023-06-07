using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace spectflow_ci_project
{
    [Binding]
    public class ListAllCategoriesStepDefinitions
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        [When(@"I send a GET request category")]
        public void WhenISendAGETRequestTo()
        {
            client = new RestClient("http://demostore.gatling.io/api");
            request = new RestRequest("category", Method.Get);
            response = client.Execute(request);
        }

        [Then(@"I expect to receive a valid response with status code OK")]
        public void ThenIExpectToReceiveAValidResponseWithStatusCodeOK()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"the response body contains a list of categories")]
        public void ThenTheResponseBodyContainsAListOfCategories()
        {
            var content = response.Content;
            Assert.IsTrue(!string.IsNullOrEmpty(content));

            try
            {
                JArray categoriesArray = JArray.Parse(content);
                foreach (var category in categoriesArray)
                {
                    int id = category.Value<int>("id");
                    string name = category.Value<string>("name");

                    Console.WriteLine($"Category ID: {id}");
                    Console.WriteLine($"Name: {name}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to parse response content as a list of categories. Error: {ex.Message}");
            }
        }
    }
}
