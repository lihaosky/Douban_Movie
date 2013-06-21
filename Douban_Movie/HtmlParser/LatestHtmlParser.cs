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
using System.Windows;
using PanoramaApp2.Resources;

namespace PanoramaApp2.HtmlParser
{
    class LatestHtmlParser
    {
        public static LongListSelector selector;
        public static ProgressBar progressbar;
        public static BoolObject loaded { get; set; }

        public static void parseLatestMovie() {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadLatestCompleted;
            client.DownloadStringAsync(new Uri(Movie.latest));
        }

        public static void downloadLatestCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    List<Movie> movieList = new List<Movie>();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='item mod odd']");
                    if (nodeCollection != null)
                    {
                        foreach (HtmlNode node in nodeCollection)
                        {
                            Movie movie;
                            try
                            {
                                movie = getLatestMovie(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            movieList.Add(movie);
                        }
                    }
                    nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='item mod ']");
                    if (nodeCollection != null) 
                    {
                        foreach (HtmlNode node in nodeCollection)
                        {
                            Movie movie;
                            try
                            {
                                movie = getLatestMovie(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            movieList.Add(movie);
                        }
                    }
                    selector.ItemsSource = movieList;
                    loaded.isLoaded = true;
                    if (progressbar != null)
                    {
                        progressbar.Visibility = Visibility.Collapsed;
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
                            parseLatestMovie();
                        }
                    }
                    else
                    {
                        if (progressbar != null)
                        {
                            progressbar.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (WebException)
            {
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
            loaded.isLoading = false;
        }

        public static Movie getLatestMovie(HtmlNode node)
        {
            string id = "";
            string title = "";
            string region = "";
            string genre = "";
            string releaseDate = "";
            string posterUrl = "";
            try
            {
                string link = node.SelectNodes("a[@class='thumb']")[0].Attributes["href"].Value;
                posterUrl = node.SelectNodes("a[@class='thumb']")[0].SelectNodes("img")[0].Attributes["src"].Value;
                id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
                HtmlNode introNode = node.SelectNodes("div[@class='intro']")[0];
                title = introNode.SelectNodes("h3")[0].SelectNodes("a")[0].InnerText;
                HtmlNode ulNode = introNode.SelectNodes("ul")[0];
                HtmlNodeCollection liNodes = ulNode.SelectNodes("li[@class='dt']");
                releaseDate = liNodes[0].InnerText;
                genre = liNodes[1].InnerText;
                region = liNodes[2].InnerText;
            }
            catch (Exception)
            {
                throw;
            }
            Movie movie = new Movie();
            movie.id = id;
            movie.genre = genre;
            movie.region = region;
            movie.posterUrl = posterUrl;
            movie.releaseDate = releaseDate;
            movie.title = title;
            return movie;
        }
    }
}
