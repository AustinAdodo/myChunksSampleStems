//using Newtonsoft.Json;

namespace WebApiExtension
{
    public class UserData
    {
        public string username { get; set; }
        public int submission_count { get; set; }
    }
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();