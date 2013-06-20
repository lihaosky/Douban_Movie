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
using PanoramaApp2.JsonParser;
using PanoramaApp2.Resources;

namespace PanoramaApp2.JsonParser
{
    class USBoxJsonParser
    {
        public static LongListSelector usboxLongListSelector;
        public static ProgressBar progressBar;
        public static BoolObject loaded { get; set; }

        public static void parseUSBox()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadUSBoxCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiUSBoxHeader + "?apikey=" + App.apikey));
        }

        public static void downloadUSBoxCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
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
                        movie.id = JsonParsers.getDouble(array[i], "subject", "id");
                        movie.posterUrl = JsonParsers.getTriple(array[i], "subject", "images", "small");
                        movie.money = JsonParsers.getValue(array[i], "box");
                        movie.rating = JsonParsers.getTriple(array[i], "subject", "rating", "average");
                        movie.title = JsonParsers.getDouble(array[i], "subject", "title");
                        movie.star = Util.getStarPath(movie.rating);
                        movieList.Add(movie);
                    }
                    usboxLongListSelector.ItemsSource = movieList;
                    if (progressBar != null)
                    {
                        progressBar.Visibility = Visibility.Collapsed;
                    }
                    loaded.isLoaded = true;
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            parseUSBox();
                        }
                    }
                    else
                    {
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
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
            loaded.isLoading = false;
        }
    }
}
