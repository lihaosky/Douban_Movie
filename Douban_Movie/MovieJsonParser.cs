using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
using System.Windows;

namespace PanoramaApp2
{
    class MovieJsonParser
    {
        public Movie movie { get; set; }
        public Image posterImage { get; set; }
        public Image starImage { get; set; }
        public TextBlock title { get; set; }
        public TextBlock rating { get; set; }
        public TextBlock rateNumber { get; set; }
        public TextBlock year_duration { get; set; }
        public TextBlock name { get; set; }
        public TextBlock region { get; set; }
        public TextBlock genre { get; set; }
        public HyperlinkButton trailer { get; set; }
        public HyperlinkButton theater { get; set; }
        public TextBlock summary { get; set; }
        public LongListSelector peopleList { get; set; }
        public WebClient client;

        public void getMovieByID()
        {
            client = new WebClient();
            client.DownloadStringCompleted += downloadJsonCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiMovieHeader + movie.id + "?apikey=" + App.apikey));
        }

        private void downloadJsonCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                string data = e.Result;
                JObject obj = JObject.Parse(data);
                movie.summary = JsonParser.getValue(obj, "summary");
                if (movie.genre == "" || movie.genre == null)
                {
                    movie.genre = JsonParser.getArray(obj, "genres");
                }
                if (movie.title == "" || movie.title == null)
                {
                    movie.title = JsonParser.getValue(obj, "title");
                }
                if (movie.year == "" || movie.year == null)
                {
                    movie.year = JsonParser.getValue(obj, "year");
                }
                if (movie.rating == "" || movie.rating == null)
                {
                    movie.rating = JsonParser.getDouble(obj, "rating", "average");
                }
                movie.star = Util.getStarPath(movie.rating);
                if (movie.rateNumber == "" || movie.rateNumber == null)
                {
                    movie.rateNumber = JsonParser.getValue(obj, "ratings_count");
                }
                if (movie.posterUrl == "" || movie.posterUrl == null)
                {
                    movie.posterUrl = JsonParser.getDouble(obj, "images", "small");
                }
                object[] countries = obj["countries"].ToArray();
                if (movie.region == "" || movie.region == null)
                {
                    movie.region = JsonParser.getArray(obj, "countries");
                }
                title.Text = movie.title;
                posterImage.Source = new BitmapImage(new Uri(movie.posterUrl));
                starImage.Source = new BitmapImage(new Uri(movie.star, UriKind.Relative));
                rating.Text = movie.rating;
                rateNumber.Text = movie.rateNumber;
                year_duration.Text = movie.year + " / " + movie.length;
                name.Text = "人评分";
                region.Text = movie.region;
                genre.Text = movie.genre;
                summary.Text = movie.summary;
                trailer.Content = "预告片";
                trailer.NavigateUri = new Uri(Movie.movieLinkHeader + movie.id + "/trailer", UriKind.Absolute);
                theater.Content = "选座购票";
                theater.NavigateUri = new Uri(Movie.movieLinkHeader + movie.id + "/cinema", UriKind.Absolute);

                List<People> peoples = new List<People>();
                JArray array = (JArray)obj["directors"];
                for (int i = 0; i < array.Count; i++)
                {
                    People people = new People();
                    people.posterUrl = JsonParser.getDouble(array[i], "avatars", "small");
                    people.id = JsonParser.getValue(array[i], "id");
                    people.name = JsonParser.getValue(array[i], "name");
                    people.positionName = "导演";
                    people.position = People.DIRECTOR;
                    peoples.Add(people);
                }
                array = (JArray)obj["casts"];
                for (int i = 0; i < array.Count; i++)
                {
                    People people = new People();
                    people.posterUrl = JsonParser.getDouble(array[i], "avatars", "small");
                    people.id = JsonParser.getValue(array[i], "id");
                    people.name = JsonParser.getValue(array[i], "name");
                    people.positionName = "";
                    people.position = People.ACTOR;
                    peoples.Add(people);
                }
                peopleList.ItemsSource = peoples;

                if (SystemTray.ProgressIndicator != null)
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
                SystemTray.IsVisible = false;
            }
            else
            {

                if (SystemTray.ProgressIndicator != null)
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
                SystemTray.IsVisible = false;
            }
        }

        public void cancelDownload()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }
    }
}
