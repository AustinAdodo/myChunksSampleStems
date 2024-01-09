//using Newtonsoft.Json;

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
}








//var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseBody);
//string[] dataArray = jsonResult.data.ToObject<string[]>();