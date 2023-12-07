using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApiExtension;
using System.Text.Json;

namespace TestForFruitsApi
{
    public class StemServiceFactory : WebApplicationFactory<Program>
    {
        public StemServiceFactory()
        {
         //additional settings
         //mocking
        }
    }

    class Result
    {
        public static string ipTracker(string ip)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string uri = $"https://jsonmock.hackerrank.com/api/ip?ip={ip}";
                    HttpResponseMessage response = client.GetAsync(uri).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Using System.Text.Json for JSON parsing
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    var root = jsonDocument.RootElement;

                    // Extracting the country
                    var test = root.TryGetProperty("data", out var dataProperty);
                    if (!test)
                    {
                        return "No Result Found";
                    }

                    var countryElement = root.GetProperty("data").EnumerateArray().FirstOrDefault().GetProperty("country");
                    string country = countryElement.GetString();

                    if (country != null)
                    {
                        return country;
                    }
                    else
                    {
                        return "No Result Found";
                    }
                }
                catch (Exception)
                {
                    return $"No Result Found";
                }
            }
        }
    }
}
