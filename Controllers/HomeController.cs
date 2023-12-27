using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace WebApiExtension.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : Controller
    {
        private const string uri = "https://raw.githubusercontent.com/qualified/challenge-data/master/words_alpha.txt";
        public HomeController() { }

        /// <summary>
        /// Produces a stem service result.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(type: typeof(StemServiceResult), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GET([FromQuery(Name = "stem")] string stem)
        {
            //string query = HttpContext.Request.Query["stem"].ToString();
            string query = stem;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string[] dataArray = responseBody.Split('\n');

                    if (query == "/")
                    {
                        StemServiceResult res = new StemServiceResult { Data = dataArray.ToList() };
                        var json = JsonSerializer.Serialize(res); return new OkObjectResult(json);
                    }

                    if (query == "")
                    {
                        StemServiceResult res = new StemServiceResult { Data = dataArray.ToList() };
                        var json = JsonSerializer.Serialize(res); return new OkObjectResult(json);
                    }

                    if (dataArray == null)
                    {
                        return StatusCode(404);
                    }

                    List<string> filteredArray = dataArray.Where(item => item.StartsWith(query)).ToList();
                    StemServiceResult result = new StemServiceResult { Data = filteredArray };

                    var json1 = JsonSerializer.Serialize(result);
                    return new OkObjectResult(json1);
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(404, ex.Message);
                }
            }
        }

        public static async Task<string> BestIngenre(string country, string phoneNumber)
        {
            string uri = $"https://jonmock.hackerrank.com/api/countries?name={country}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to dynamic object
                    dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                    if (jsonData.data.Count == 0)
                    {
                        return "-1"; // Country not found
                    }

                    // Extracting calling codes
                    string[] callingCodes = jsonData.data[0].callingcodes.ToObject<string[]>();

                    // Get the highest index calling code
                    string callingCode = callingCodes.Length > 0 ? callingCodes[callingCodes.Length - 1] : "";

                    // Format the phone number
                    string formattedPhoneNumber = $"+{callingCode} {phoneNumber}";

                    return formattedPhoneNumber;
                }
                else
                {
                    return "-1"; // API request failed
                }
            }
        }

    }
}







//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();