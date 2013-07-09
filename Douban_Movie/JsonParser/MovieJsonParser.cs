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
using PanoramaApp2.JsonParser;
using PanoramaApp2.Resources;
using Phone.Controls;

namespace PanoramaApp2
{
    class MovieJsonParser
    {
        private Movie movie;
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
        public ScrollableTextBlock summary { get; set; }
        public LongListSelector peopleList { get; set; }
        public ProgressBar progressBar { get; set; }
        public WebClient client;

        public MovieJsonParser(Movie m)
        {
            movie = m;
        }

        public void getMovieByID()
        {
            Movie result = Cache.getMovie(movie.id);
            if (result != null)
            {
                movie = result;
                setUI();
                peopleList.ItemsSource = movie.peopleList;
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                client = new WebClient();
                client.DownloadStringCompleted += downloadJsonCompleted;
                client.DownloadStringAsync(new Uri(Movie.apiMovieHeader + movie.id + "?apikey=" + App.apikey));
            }
        }

        private void downloadJsonCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string data = e.Result;
                    JObject obj = JObject.Parse(data);
                    movie.summary = JsonParsers.getValue(obj, "summary");
                    if (movie.genre == "" || movie.genre == null)
                    {
                        movie.genre = JsonParsers.getArray(obj, "genres");
                    }
                    if (movie.title == "" || movie.title == null)
                    {
                        movie.title = JsonParsers.getValue(obj, "title");
                    }
                    if (movie.year == "" || movie.year == null)
                    {
                        movie.year = JsonParsers.getValue(obj, "year");
                    }
                    if (movie.rating == "" || movie.rating == null)
                    {
                        movie.rating = JsonParsers.getDouble(obj, "rating", "average");
                    }
                    movie.star = Util.getStarPath(movie.rating);
                    if (movie.rateNumber == "" || movie.rateNumber == null)
                    {
                        movie.rateNumber = JsonParsers.getValue(obj, "ratings_count");
                    }
                    if (movie.posterUrl == "" || movie.posterUrl == null)
                    {
                        movie.posterUrl = JsonParsers.getDouble(obj, "images", "small");
                    }
                    object[] countries = obj["countries"].ToArray();
                    if (movie.region == "" || movie.region == null)
                    {
                        movie.region = JsonParsers.getArray(obj, "countries");
                    }

                    if (movie.posterUrl == "")
                    {
                        movie.posterUrl = App.imagePath + "default.png";
                    }
                    setUI();

                    List<People> peoples = new List<People>();
                    JArray array = (JArray)obj["directors"];
                    for (int i = 0; i < array.Count; i++)
                    {
                        People people = new People();
                        people.posterUrl = JsonParsers.getDouble(array[i], "avatars", "small");
                        if (people.posterUrl == "")
                        {
                            people.posterUrl = App.imagePath + "default.png";
                        }
                        people.id = JsonParsers.getValue(array[i], "id");
                        people.name = JsonParsers.getValue(array[i], "name");
                        people.positionName = "导演";
                        people.position = People.DIRECTOR;
                        peoples.Add(people);
                    }
                    array = (JArray)obj["casts"];
                    for (int i = 0; i < array.Count; i++)
                    {
                        People people = new People();
                        people.posterUrl = JsonParsers.getDouble(array[i], "avatars", "small");
                        if (people.posterUrl == "")
                        {
                            people.posterUrl = App.imagePath + "default.png";
                        }
                        people.id = JsonParsers.getValue(array[i], "id");
                        people.name = JsonParsers.getValue(array[i], "name");
                        people.positionName = "";
                        people.position = People.ACTOR;
                        peoples.Add(people);
                    }
                    movie.peopleList = peoples;
                    peopleList.ItemsSource = peoples;
                    // Insert movie into cache
                    Cache.insertMovie(movie);
                    if (progressBar != null)
                    {
                        progressBar.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            getMovieByID();
                        }
                    }
                    if (progressBar != null)
                    {
                        progressBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (WebException)
            {
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void setUI()
        {
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
