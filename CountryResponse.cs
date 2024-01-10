//using Newtonsoft.Json;

namespace WebApiExtension
{
    public class CountryResponse
    {
        public string name { get; set; }
        public string[] callingCodes { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();