using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Phone.Controls;
using HtmlAgilityPack;
using System.Windows.Controls.Primitives;
using System.Windows;
using PanoramaApp2.Resources;

namespace PanoramaApp2.HtmlParser
{
    /// <summary>
    /// This class parses html file and returns corresponding results
    /// </summary>
    class HotMovieHtmlParser
    {
        public static LongListSelector selector { get; set; }
        public static Popup popup { get; set; }
        public static BoolObject loaded { get; set; }

        /// <summary>
        /// Return a list of latest movies 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void parseHottMovie()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadLatestMovieCompleted;
            client.DownloadStringAsync(new Uri(Movie.homePage));
        }

        /// <summary>
        /// Download movie info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void downloadLatestMovieCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && e.Error == null)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//li[@class='ui-slide-item s']");
                    List<Movie> movieList = new List<Movie>();

                    // Can't find movie! Hmmm, shouldn't happen...
                    if (nodeCollection != null)
                    {
                        foreach (HtmlNode movieNode in nodeCollection)
                        {
                            Movie movie;
                            try
                            {
                                movie = getHotMovie(movieNode);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            movieList.Add(movie);
                        }
                    }
                    nodeCollection = doc.DocumentNode.SelectNodes("//li[@class='ui-slide-item']");
                    // Can't find movie! Hmmm, shouldn't happen...
                    if (nodeCollection != null)
                    {
                        foreach (HtmlNode movieNode in nodeCollection)
                        {
                            Movie movie;
                            try
                            {
                                movie = getHotMovie(movieNode);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            movieList.Add(movie);
                        }
                    }
                    loaded.isLoaded = true;
                    selector.ItemsSource = movieList;
                    popup.IsOpen = false;
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            parseHottMovie();
                        }
                    }
                    else
                    {
                        popup.IsOpen = false;
                    }
                }
            }
            catch (WebException)
            {
                popup.IsOpen = false;
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
            loaded.isLoading = false;
        }

        /// <summary>
        /// Get movie from movieNode, if anything wrong throw exception
        /// </summary>
        /// <param name="movieNode"></param>
        /// <returns></returns>
        private static Movie getHotMovie(HtmlNode movieNode)
        {
            string rate = "";
            string actors = "";
            string title = "";
            string link = "";
            string imgLink = "";
            string alt_title = "";
            string length = "";
            string year = "";
            string region = "";
            string rateNumber = "";

            try
            {
                rate = movieNode.Attributes["data-rate"].Value.Trim();
                actors = movieNode.Attributes["data-actors"].Value.Trim();
                title = movieNode.Attributes["data-title"].Value.Trim();
                length = movieNode.Attributes["data-duration"].Value.Trim();
                year = movieNode.Attributes["data-release"].Value.Trim();
                region = movieNode.Attributes["data-region"].Value.Trim();
                rateNumber = movieNode.Attributes["data-rater"].Value.Trim();
                HtmlNode liNode = movieNode.SelectNodes("ul/li[@class='poster']")[0];
                HtmlNode aNode = liNode.SelectNodes("a")[0];
                link = aNode.Attributes["href"].Value.Trim();
                HtmlNode imgNode = aNode.SelectNodes("img")[0];
                imgLink = imgNode.Attributes["src"].Value.Trim();
                foreach (HtmlAttribute attr in imgNode.Attributes)
                {
                    if (attr.Name == "data-original")
                    {
                        imgLink = attr.Value.Trim();
                        break;
                    }
                }

                alt_title = imgNode.Attributes["alt"].Value.Trim();
            }
            catch (System.NullReferenceException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            Movie movie = new Movie();
            movie.rating = rate;
            movie.posterUrl = imgLink;
            movie.actors_list = Util.replaceSpecialChar(actors);
            movie.title = Util.replaceSpecialChar(alt_title);
            movie.length = length;
            movie.year = year;
            movie.region = Util.replaceSpecialChar(region);
            movie.rateNumber = rateNumber;
            movie.id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
            movie.star = Util.getStarPath(movie.rating);
            return movie;
        }
    }
}
