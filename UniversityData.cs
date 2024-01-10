//using Newtonsoft.Json;

namespace WebApiExtension
{
    public class UniversityData
    {
        public string university { get; set; }
        public string International_students { get; set; }
        public Location location { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();