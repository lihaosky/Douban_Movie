﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;

namespace PanoramaApp2
{
    class Top250HtmlParser
    {
        public LongListSelector selector;

        public void parseTop250()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadTop250Completed;
            client.DownloadStringAsync(new Uri(Movie.top250));
            
        }

        public void downloadTop250Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            string page = e.Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='item']");
            List<Movie> movieList = new List<Movie>();
            if (nodeCollection == null)
            {
                System.Diagnostics.Debug.WriteLine("null collection!");
            }
            else
            {
                foreach (HtmlNode node in nodeCollection)
                {
                    Movie movie;
                    try
                    {
                        movie = getTopMovie(node);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    movieList.Add(movie);
                }
            }
            selector.ItemsSource = movieList;
        }

        private Movie getTopMovie(HtmlNode node)
        {
            string id = "";
            string title = "";
            string posterUrl = "";
            string rating = "";
            string quote = "";

            try
            {
                HtmlNode linkNode = node.SelectNodes("div[@class='pic']")[0].SelectNodes("a")[0];
                string link = linkNode.Attributes["href"].Value;
                id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
                HtmlNode imgNode = linkNode.SelectNodes("img")[0];
                posterUrl = imgNode.Attributes["src"].Value;
                title = imgNode.Attributes["alt"].Value;
                HtmlNode bdNode = node.SelectNodes("div[@class='info']")[0].SelectNodes("div[@class='bd']")[0];
                HtmlNodeCollection ratingNodes = bdNode.SelectNodes("div[@class='star']")[0].SelectNodes("span[@class='rating5-t']");
                if (ratingNodes == null)
                {
                    ratingNodes = bdNode.SelectNodes("div[@class='star']")[0].SelectNodes("span[@class='rating45-t']");
                }
                rating = ratingNodes[0].SelectNodes("em")[0].InnerText;
                quote = bdNode.SelectNodes("p[@class='quote']")[0].SelectNodes("span")[0].InnerText;
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
            movie.id = id;
            movie.title = title;
            movie.rating = rating;
            movie.star = Util.getStarPath(rating);
            movie.posterUrl = posterUrl;
            movie.quote = quote;
            return movie;
        }
    }
}
