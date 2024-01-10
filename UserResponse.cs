//using Newtonsoft.Json;

namespace WebApiExtension
{
    public class UserResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public UserData[] data { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();