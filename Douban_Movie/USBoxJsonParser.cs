using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace PanoramaApp2
{
    class USBoxJsonParser
    {
        public static LongListSelector usboxLongListSelector;
        public static ProgressIndicator indicator;

        public static void parseUSBox()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadUSBoxCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiUSBoxHeader + "?apikey=" + App.apikey));
        }

        public static void downloadUSBoxCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                string data = e.Result;
                JObject obj = JObject.Parse(data);
                JArray array = (JArray)obj["subjects"];
                List<Movie> movieList = new List<Movie>();
                for (int i = 0; i < array.Count; i++)
                {
                    Movie movie = new Movie();
                    movie.id = JsonParser.getDouble(array[i], "subject", "id");
                    movie.posterUrl = JsonParser.getTriple(array[i], "subject", "images", "small");
                    movie.money = JsonParser.getValue(array[i], "box");
                    movie.rating = JsonParser.getTriple(array[i], "subject", "rating", "average");
                    movie.title = JsonParser.getDouble(array[i], "subject", "title");
                    movie.star = Util.getStarPath(movie.rating);
                    movieList.Add(movie);
                }
                usboxLongListSelector.ItemsSource = movieList;
                if (indicator != null)
                {
                    indicator.IsVisible = false;
                }
                SystemTray.IsVisible = false;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("无法连接到豆瓣网,请检查网络连接", "", MessageBoxButton.OK);
                if (indicator != null)
                {
                    indicator.IsVisible = false;
                }
                SystemTray.IsVisible = false;
            }
        }
    }
}
