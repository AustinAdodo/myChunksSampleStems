//using Newtonsoft.Json;

namespace WebApiExtension.Controllers
{
    public class Location
    {
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();