using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PanoramaApp2
{
    class USBoxJsonParser
    {
        public static LongListSelector usboxLongListSelector;

        public static void parseUSBox()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadUSBoxCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiUSBoxHeader + "?apikey=" + App.apikey));
        }

        public static void downloadUSBoxCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
            JObject obj = JObject.Parse(data);
            JArray array = (JArray)obj["subjects"];
            List<Movie> movieList = new List<Movie>();
            for (int i = 0; i < array.Count; i++)
            {
                Movie movie = new Movie();
                movie.id = (string)array[i]["subject"]["id"];
                movie.posterUrl = (string)array[i]["subject"]["images"]["medium"];
                movie.money = (string)array[i]["box"];
                movie.rating = (string)array[i]["subject"]["rating"]["average"];
                movie.title = (string)array[i]["subject"]["title"];
                movie.star = Util.getStarPath(movie.rating);
                movieList.Add(movie);
            }
            usboxLongListSelector.ItemsSource = movieList;
        }
    }
}
