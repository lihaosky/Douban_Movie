using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Phone.Controls;
using HtmlAgilityPack;
using System.Windows.Controls.Primitives;

namespace PanoramaApp2
{
    /// <summary>
    /// This class parses html file and returns corresponding results
    /// </summary>
    class HtmlParser
    {
        public LongListSelector selector { get; set; }
        public Popup popup { get; set; }

        /// <summary>
        /// Return a list of latest movies 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void parseHottMovie()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadLatestMovieCompleted;
            client.DownloadStringAsync(new Uri("http://movie.douban.com"));
        }

        /// <summary>
        /// Download movie info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadLatestMovieCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string page = e.Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//li[@class='ui-slide-item s']");
            List<Movie> movieList = new List<Movie>();

            // Can't find movie! Hmmm, shouldn't happen...
            if (nodeCollection == null)
            {
                System.Diagnostics.Debug.WriteLine("null nodeCollection!");
            }
            else
            {
                foreach (HtmlNode movieNode in nodeCollection)
                {
                    string rate;
                    string actors;
                    string title;
                    string link;
                    string imgLink;
                    string alt_title;

                    try
                    {
                        rate = movieNode.Attributes["data-rate"].Value;
                        actors = movieNode.Attributes["data-actors"].Value;
                        title = movieNode.Attributes["data-title"].Value;
                        HtmlNode liNode = movieNode.SelectNodes("ul/li[@class='poster']")[0];
                        HtmlNode aNode = liNode.SelectNodes("a")[0];
                        link = aNode.Attributes["href"].Value;
                        HtmlNode imgNode = aNode.SelectNodes("img")[0];
                        imgLink = imgNode.Attributes["src"].Value;
                        foreach (HtmlAttribute attr in imgNode.Attributes)
                        {
                            if (attr.Name == "data-original")
                            {
                                imgLink = attr.Value;
                                break;
                            }
                        }
                        alt_title = imgNode.Attributes["alt"].Value;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    Movie movie = new Movie();
                    movie.rating = rate;
                    movie.posterUrl = imgLink;
                    movie.actors_list = actors;
                    movie.title = alt_title;
                    movie.id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
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
                    string starPath = "/PanoramaApp2;component/Assets/" + baseStar + "";
                    if (half)
                    {
                        starPath += 5;
                    }
                    starPath += " star.png";
                    movie.star = starPath;
                    movieList.Add(movie);
                }
            }
            nodeCollection = doc.DocumentNode.SelectNodes("//li[@class='ui-slide-item']");
            // Can't find movie! Hmmm, shouldn't happen...
            if (nodeCollection == null)
            {
                System.Diagnostics.Debug.WriteLine("null nodeCollection!");
            }
            else
            {
                foreach (HtmlNode movieNode in nodeCollection)
                {
                    string rate;
                    string actors;
                    string title;
                    string link;
                    string imgLink;
                    string alt_title;

                    try
                    {
                        rate = movieNode.Attributes["data-rate"].Value;
                        actors = movieNode.Attributes["data-actors"].Value;
                        title = movieNode.Attributes["data-title"].Value;
                        HtmlNode liNode = movieNode.SelectNodes("ul/li[@class='poster']")[0];
                        HtmlNode aNode = liNode.SelectNodes("a")[0];
                        link = aNode.Attributes["href"].Value;
                        HtmlNode imgNode = aNode.SelectNodes("img")[0];
                        imgLink = imgNode.Attributes["src"].Value;
                        foreach (HtmlAttribute attr in imgNode.Attributes)
                        {
                            if (attr.Name == "data-original")
                            {
                                imgLink = attr.Value;
                                break;
                            }
                        }
                        alt_title = imgNode.Attributes["alt"].Value;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    Movie movie = new Movie();
                    movie.rating = rate;
                    movie.posterUrl = imgLink;
                    movie.actors_list = actors;
                    movie.title = alt_title;
                    movie.id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
                    System.Diagnostics.Debug.WriteLine(rate);
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
                    string starPath = "/PanoramaApp2;component/Assets/" + baseStar + "";
                    if (half)
                    {
                        starPath += 5;
                    }
                    starPath += " star.png";
                    movie.star = starPath;
                    movieList.Add(movie);
                }
            }
            selector.ItemsSource = movieList;
            popup.IsOpen = false;
        }
    }
}
