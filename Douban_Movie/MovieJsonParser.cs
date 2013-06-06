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
                movie.summary = (string)obj["summary"];
                object[] genres = obj["genres"].ToArray();
                for (int i = 0; i < genres.Length - 1; i++)
                {
                    movie.genre += genres[i].ToString();
                    movie.genre += " / ";
                }
                movie.genre += genres[genres.Length - 1].ToString();
                movie.title = (string)obj["title"];
                movie.year = (string)obj["year"];
                movie.rating = (string)obj["rating"]["average"];
                movie.star = Util.getStarPath(movie.rating);
                movie.rateNumber = (string)obj["ratings_count"];
                if (movie.posterUrl == "" || movie.posterUrl == null)
                {
                    movie.posterUrl = (string)obj["images"]["medium"];
                }
                object[] countries = obj["countries"].ToArray();
                movie.region = "";
                for (int i = 0; i < countries.Length - 1; i++)
                {
                    movie.region += countries[i].ToString();
                    movie.region += " / ";
                }
                movie.region += countries[countries.Length - 1].ToString();
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
                if (SystemTray.ProgressIndicator != null)
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
                SystemTray.IsVisible = false;
            }
        }

        public void cancelDownLoad()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }
    }
}
