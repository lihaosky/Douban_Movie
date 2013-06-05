﻿using System;
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
    class HotMovieHtmlParser
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
            client.DownloadStringAsync(new Uri(Movie.homePage));
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
            if (nodeCollection == null)
            {
                System.Diagnostics.Debug.WriteLine("null nodeCollection!");
            }
            else
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
            selector.ItemsSource = movieList;
            popup.IsOpen = false;
        }

        /// <summary>
        /// Get movie from movieNode, if anything wrong throw exception
        /// </summary>
        /// <param name="movieNode"></param>
        /// <returns></returns>
        private Movie getHotMovie(HtmlNode movieNode)
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
                rate = movieNode.Attributes["data-rate"].Value;
                actors = movieNode.Attributes["data-actors"].Value;
                title = movieNode.Attributes["data-title"].Value;
                length = movieNode.Attributes["data-duration"].Value;
                year = movieNode.Attributes["data-release"].Value;
                region = movieNode.Attributes["data-region"].Value;
                rateNumber = movieNode.Attributes["data-rater"].Value;
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
                throw;
            }
            Movie movie = new Movie();
            movie.rating = rate;
            movie.posterUrl = imgLink;
            movie.actors_list = actors;
            movie.title = alt_title;
            movie.length = length;
            movie.year = year;
            movie.region = region;
            movie.rateNumber = rateNumber;
            movie.id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
            movie.star = Util.getStarPath(movie.rating);
            return movie;
        }
    }
}