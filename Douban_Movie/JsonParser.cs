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

namespace PanoramaApp2
{
    class JsonParser
    {
        public Movie movie { get; set; }
        public Image posterImage { get; set; }
        public Image starImage { get; set; }
        public TextBlock title { get; set; }
        public TextBlock rating { get; set; }
        public TextBlock rateNumber { get; set; }
        public TextBlock year_duration { get; set; }
        public ProgressBar progressbar { get; set; }
        public TextBlock name { get; set; }
        public TextBlock region { get; set; }
        public TextBlock genre { get; set; }
        public HyperlinkButton trailer { get; set; }
        public HyperlinkButton theater { get; set; }
        public TextBlock summary { get; set; }

        public void getMovieByID()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadJsonCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiMovieHeader + movie.id + "?apikey=" + App.apikey));
        }

        public void downloadJsonCompleted(object sender, DownloadStringCompletedEventArgs e)
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
            movie.star = getStarPath(movie.rating);
            movie.rateNumber = (string)obj["ratings_count"];
            if (movie.posterUrl == "" || movie.posterUrl == null)
            {
                movie.posterUrl = (string)obj["images"]["medium"];
            }
            object[] countries = obj["countries"].ToArray();
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
            trailer.Content = "花絮";
            trailer.NavigateUri = new Uri(Movie.movieLinkHeader + movie.id + "/trailer", UriKind.Absolute);
            theater.Content = "选座购票";
            theater.NavigateUri = new Uri(Movie.movieLinkHeader + movie.id + "/cinema", UriKind.Absolute);
            progressbar.IsIndeterminate = false;
            progressbar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private string getStarPath(string rate)
        {
            double rating = 0;
            try
            {
                rating = double.Parse(rate);
            }
            catch (System.FormatException)
            {
            }
            double stars = rating / 2.0;
            int baseStar = (int)stars;
            int roundStar = (int)Math.Round(stars);
            bool half = roundStar > baseStar ? true : false;
            string starPath = App.imagePath + baseStar;
            if (half)
            {
                starPath += 5;
            }
            starPath += " star.png";
            return starPath;
        }
    }
}
