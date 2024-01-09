using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using System.Text.Json;


namespace WebApiExtension.Controllers
{
    public class TVSeries
    {
        public string name { get; set; }
        public string runtime_of_series { get; set; }
        public string runtime_of_episodes { get; set; }
        public string genre { get; set; }
        public float imdb_rating { get; set; }
        public string overview { get; set; }
        public int no_of_votes { get; set; }
        public int id { get; set; }
    }
    public class UserData
    {
        public string username { get; set; }
        public int submission_count { get; set; }
    }
    public class UserResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public UserData[] data { get; set; }
    }
    public class Location
    {
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
    }
    public class UniversityData
    {
        public string university { get; set; }
        public string International_students { get; set; }
        public Location location { get; set; }
    }
    public class UniversityResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public UniversityData[] data { get; set; }
    }
    public class CountryResponse
    {
        public string name { get; set; }
        public string[] callingCodes { get; set; }
    }
    public class CountryData
    {
        public CountryResponse[] data { get; set; }
    }



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

        /// <summary>
        /// Getting Phone Numbers.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public async Task<string> getPhoneNumbers(string country, string phoneNumber)
        {
            string uri = $"https://jonmock.hackerrank.com/api/countries?name={country}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to dynamic object
                    // use other library outsid newtonsoft
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

        /// <summary>
        /// Filters genre from an API.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static async Task<string> BestIngenre(string genre, int num)
        {
            string uri = $"https://jonmock.hackerrank.com/api/tvseries?page={num}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to a collection of TVSeries objects
                    var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<TVSeries[]>(responseBody);

                    // Filter TV series by genre
                    var seriesInGenre = jsonData.Where(series => series.genre.Contains(genre));

                    // Find the series with the highest IMDb rating
                    var highestRatedSeries = seriesInGenre.OrderByDescending(series => series.imdb_rating)
                        .ThenBy(series => series.name)
                        .FirstOrDefault();

                    if (highestRatedSeries != null)
                    {
                        string seriesName = highestRatedSeries.name;
                        string seriesRating = highestRatedSeries.imdb_rating.ToString();

                        return $"{seriesName} has the highest IMDb rating ({seriesRating}) in the genre '{genre}'.";
                    }
                    else
                    {
                        return $"No TV series found in the genre '{genre}' on page {num}.";
                    }
                }
                else
                {
                    return $"Failed to retrieve TV series data from the API on page {num}.";
                }
            }
        }

        /// <summary>
        /// Retrieves calling codes from provided Phone numbers.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static string GetPhoneNumbers(string country, string phoneNumber)
        {
            string uri = $"https://jsonmock.hackerrank.com/api/countries?name={country}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = response.Content.ReadAsStringAsync().Result;

                    CountryData countryData = Newtonsoft.Json.JsonConvert.DeserializeObject<CountryData>(jsonContent);

                    if (countryData.data.Length == 0 || countryData.data[0].callingCodes.Length == 0)
                    {
                        return "-1"; // Country not found or no calling codes available
                    }
                    else
                    {
                        string callingCode = countryData.data[0].callingCodes[^1]; // Get the code at the highest index
                        string formattedPhoneNumber = $"+{callingCode} {phoneNumber}";
                        return formattedPhoneNumber;
                    }
                }
                else
                {
                    return "-1"; // Failed API request
                }
            }
        }

        /// <summary>
        /// Retrieves data of some Universities.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static async Task<string> highestInternationalStudents(string firstCity, string secondCity)
        {
            string uri = "https://jsonmock.hackerrank.com/api/universities";

            string firstCityUniversity = await GetUniversityWithMostInternationalStudents(uri, firstCity);
            if (!string.IsNullOrEmpty(firstCityUniversity))
            {
                return firstCityUniversity;
            }

            string secondCityUniversity = await GetUniversityWithMostInternationalStudents(uri, secondCity);
            return secondCityUniversity;
        }

        public static async Task<string> GetUniversityWithMostInternationalStudents(string uri, string city)
        {
            string apiUrl = $"{uri}?page=1";
            using (HttpClient client = new HttpClient())
            {
                while (!string.IsNullOrEmpty(apiUrl))
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        UniversityResponse universityData = Newtonsoft.Json.JsonConvert.DeserializeObject<UniversityResponse>(jsonContent);

                        foreach (var university in universityData.data)
                        {
                            if (university.location.city == city)
                            {
                                return GetUniversityNameWithHighestInternationalStudents(universityData.data);
                            }
                        }
                        apiUrl = GetNextPageUrl(response);
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to fetch data. StatusCode: {response.StatusCode}");
                    }
                }
            }

            return null;
        }

        public static string GetUniversityNameWithHighestInternationalStudents(UniversityData[] universities)
        {
            UniversityData universityWithMaxStudents = null;
            int maxStudents = 0;

            foreach (var university in universities)
            {
                if (int.TryParse(university.International_students.Replace(",", ""), out int currentStudents))
                {
                    if (currentStudents > maxStudents)
                    {
                        maxStudents = currentStudents;
                        universityWithMaxStudents = university;
                    }
                }
            }
            return universityWithMaxStudents?.university;
        }

        public static string GetNextPageUrl(HttpResponseMessage response)
        {
            string linkHeader = response.Headers.GetValues("Link").FirstOrDefault();
            if (!string.IsNullOrEmpty(linkHeader))
            {
                string[] links = linkHeader.Split(',');
                foreach (string link in links)
                {
                    string[] parts = link.Split(';');
                    if (parts.Length == 2 && parts[1].Trim() == "rel=\"next\"")
                    {
                        return parts[0].Trim('<', '>');
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves usernames of active users depending on a threshold.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static async Task<List<string>> GetUsernames(int threshold)
        {
            string uri = $"https://jsonmock.hackerrank.com/api/article_users?page=1";
            List<string> activeUsernames = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                while (!string.IsNullOrEmpty(uri))
                {
                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        UserResponse userResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponse>(jsonContent);

                        foreach (var userData in userResponse.data)
                        {
                            if (userData.submission_count > threshold)
                            {
                                activeUsernames.Add(userData.username);
                            }
                        }
                        uri = GetNextPageUrl1(response);
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to fetch data. StatusCode: {response.StatusCode}");
                    }
                }
            }
            return activeUsernames;
        }
        public static string GetNextPageUrl1(HttpResponseMessage response)
        {
            string linkHeader = response.Headers.GetValues("Link").FirstOrDefault();
            if (!string.IsNullOrEmpty(linkHeader))
            {
                string[] links = linkHeader.Split(',');
                foreach (string link in links)
                {
                    string[] parts = link.Split(';');
                    if (parts.Length == 2 && parts[1].Trim() == "rel=\"next\"")
                    {
                        return parts[0].Trim('<', '>');
                    }
                }
            }
            return null;
        }

    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();