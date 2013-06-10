﻿using System;
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

namespace PanoramaApp2
{
    class Top250HtmlParser
    {
        public static Button loadMoreButton;
        public static TextBlock loadText;
        public static int currentIndex = 0;
        public static int maxIndex = 9;
        public static ProgressIndicator indicator;
        public static ObservableCollection<Movie> observableMovieList = new ObservableCollection<Movie>();

        public static void parseTop250()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadTop250Completed;
            client.DownloadStringAsync(new Uri(Movie.top250 + "?start=" + currentIndex * 25 + "&format="));
        }

        public static void downloadTop250Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='item']");
                    if (nodeCollection != null)
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
                            observableMovieList.Add(movie);
                        }
                    }
                    if (indicator != null)
                    {
                        indicator.IsVisible = false;
                    }
                    SystemTray.IsVisible = false;
                    currentIndex++;
                    if (currentIndex > maxIndex)
                    {
                        loadMoreButton.IsEnabled = false;
                        loadText.Text = "完了:-)";
                    }
                    else
                    {
                        loadMoreButton.IsEnabled = true;
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("无法连接到豆瓣网,请检查网络连接", "", MessageBoxButton.OK);
                    if (indicator != null)
                    {
                        indicator.IsVisible = false;
                    }
                    SystemTray.IsVisible = false;
                }
            }
            catch (WebException)
            {
                MessageBoxResult result = MessageBox.Show("无法连接到豆瓣网,请检查网络连接", "", MessageBoxButton.OK);
            }
        }

        private static Movie getTopMovie(HtmlNode node)
        {
            string id = "";
            string title = "";
            string posterUrl = "";
            string rating = "";
            string quote = "";

            try
            {
                HtmlNode linkNode = node.SelectNodes("div[@class='pic']")[0].SelectNodes("a")[0];
                string link = linkNode.Attributes["href"].Value.Trim();
                id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
                HtmlNode imgNode = linkNode.SelectNodes("img")[0];
                posterUrl = imgNode.Attributes["src"].Value.Trim();
                title = imgNode.Attributes["alt"].Value.Trim();
                HtmlNode bdNode = node.SelectNodes("div[@class='info']")[0].SelectNodes("div[@class='bd']")[0];
                HtmlNodeCollection ratingNodes = bdNode.SelectNodes("div[@class='star']")[0].SelectNodes("span[@class='rating5-t']");
                if (ratingNodes == null)
                {
                    ratingNodes = bdNode.SelectNodes("div[@class='star']")[0].SelectNodes("span[@class='rating45-t']");
                }
                rating = ratingNodes[0].SelectNodes("em")[0].InnerText.Trim();
                quote = bdNode.SelectNodes("p[@class='quote']")[0].SelectNodes("span")[0].InnerText.Trim();
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
            movie.title = Util.replaceSpecialChar(title);
            movie.rating = rating;
            movie.star = Util.getStarPath(rating);
            movie.posterUrl = posterUrl;
            movie.quote = Util.replaceSpecialChar(quote);
            return movie;
        }
    }
}
