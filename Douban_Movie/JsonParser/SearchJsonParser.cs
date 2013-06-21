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
    class SearchJsonParser
    {
        public LongListSelector selector { get; set; }
        public ProgressBar progressBar { get; set; }
        public TextBlock resultNumber { get; set; }
        private string searchText;
        private WebClient client;

        public void search(string text)
        {
            searchText = text;
            client = new WebClient();
            client.DownloadStringCompleted += downloadSearchCompleted;
            client.DownloadStringAsync(new Uri(Movie.apiSearchHeader + "?apikey=" + App.apikey + "&q=" + text));
        }

        public void downloadSearchCompleted(object sender, DownloadStringCompletedEventArgs e)
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
                        movie.id = JsonParsers.getValue(array[i], "id");
                        movie.posterUrl = JsonParsers.getDouble(array[i], "images", "small");
                        movie.rating = JsonParsers.getDouble(array[i], "rating", "average");
                        movie.title = JsonParsers.getValue(array[i], "title");
                        movie.star = Util.getStarPath(movie.rating);
                        movieList.Add(movie);
                    }
                    resultNumber.Text = array.Count + "";
                    selector.ItemsSource = movieList;
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
                            search(searchText);
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
