using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using NUnit.Framework;
using WebApiExtension;
using WebApiExtension.Controllers;

namespace TestForFruitsApi
{
    [TestFixture]
        public class StemsControllerTests
        {
            private StemServiceFactory _factory;
            private HttpClient _client;

            [OneTimeSetUp]
            public void SetUp()
            {
                _factory = new StemServiceFactory();
                _client = _factory.CreateClient();
            }

            [Test, Description("should return a list of words starting with aar")]
            public async Task TestGetAar()
            {
                var response = await _client.GetAsync("/?stem=aar");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                string responseBody = await response.Content.ReadAsStringAsync();
                var expected = new List<string>
            {
                "aardvark",
                "aardvarks",
                "aardwolf",
                "aardwolves",
                "aargh",
                "aaron",
                "aaronic",
                "aaronical",
                "aaronite",
                "aaronitic",
                "aarrgh",
                "aarrghh",
                "aaru"
            }; //can use Mock
                try
                {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var actual = JsonSerializer.Deserialize<StemServiceResult>(responseBody);
                    Assert.That(actual.Data, Is.EqualTo(expected));
                }
                catch (System.Text.Json.JsonException)
                {
                    Assert.Fail("Could not deserialize response JSON:\n" + Trunc(responseBody));
                }
            }

            [OneTimeTearDown]
            public void TearDown()
            {
                _client.Dispose();
                _factory.Dispose();
            }

            private static string Trunc(string s, int thresh = 200, int trunc = 50)
            {
                if (s.Length > thresh)
                {
                    return s.Substring(0, trunc) + "..." + s.Substring(s.Length - trunc);
                }
                return s;
            }
    }
    }
