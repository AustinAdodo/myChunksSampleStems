//using Newtonsoft.Json;

namespace WebApiExtension.Controllers
{
    public class CountryData
    {
        public CountryResponse[] data { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();