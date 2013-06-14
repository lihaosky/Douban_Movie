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

namespace PanoramaApp2.HtmlParser
{
    class PeopleMovieHtmlParser
    {
        private People people;
        public ProgressBar progressBar { get; set; }
        public Button button;
        public TextBlock text;
        public ObservableCollection<Movie> movieCollection = new ObservableCollection<Movie>();
        private WebClient client;

        public PeopleMovieHtmlParser(People p)
        {
            people = p;
        }

        public void parseMovie()
        {
            client = new WebClient();
            client.DownloadStringCompleted += client_DownloadMovieCompleted;
            client.DownloadStringAsync(new Uri(people.nextMovieLink));
        }

        void client_DownloadMovieCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nameNodes = doc.DocumentNode.SelectNodes("//ul");
                    if (nameNodes == null || nameNodes.Count < 4)
                    {
                        people.hasMoreMovie = false;
                        button.IsEnabled = false;
                        text.Text = "完了:-)";
                    }
                    else 
                    {
                        foreach (HtmlNode node in nameNodes[3].SelectNodes("li"))
                        {
                            Movie m;
                            try
                            {
                                m = getMovie(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            movieCollection.Add(m);
                        }
                        HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='paginator']");
                        if (nodeCollection == null)
                        {
                            people.hasMoreMovie = false;
                            button.IsEnabled = false;
                            text.Text = "完了:-)";
                        }
                        else
                        {
                            HtmlNodeCollection nc = nodeCollection[0].SelectNodes("span[@class='next']");
                            if (nc == null)
                            {
                                people.hasMoreMovie = false;
                                button.IsEnabled = false;
                                text.Text = "完了:-)";
                            }
                            else
                            {
                                HtmlNodeCollection aCollection = nc[0].SelectNodes("a");
                                if (aCollection == null)
                                {
                                    people.hasMoreMovie = false;
                                    button.IsEnabled = false;
                                    text.Text = "完了:-)";
                                }
                                else
                                {
                                    people.hasMoreMovie = true;
                                    string link = aCollection[0].Attributes["href"].Value;
                                    link = link.Replace("&amp;", "&");
                                    people.nextMovieLink = People.peopleLinkHeader + people.id + "/movies" + link;
                                    button.IsEnabled = true;
                                }
                            }
                        }
                    }
                }
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
            }
            catch (WebException)
            {
                button.IsEnabled = true;
                if (progressBar != null) {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show("无法连接到豆瓣网,请检查网络连接", "", MessageBoxButton.OK);
            }
        }

        private Movie getMovie(HtmlNode node)
        {
            string id = "";
            string name = "";
            string rating = "";
            string star = "";
            string posterUrl = "";

            try
            {
                HtmlNode dlNode = node.SelectNodes("dl")[0];
                posterUrl = dlNode.SelectNodes("dt")[0].SelectNodes("a")[0].SelectNodes("img")[0].Attributes["src"].Value.Trim();
                HtmlNode ddNode = dlNode.SelectNodes("dd")[0];
                HtmlNode aNode = ddNode.SelectNodes("h6")[0].SelectNodes("a")[0];
                string link = aNode.Attributes["href"].Value;
                name = aNode.InnerText.Trim();
                id = link.Substring(Movie.movieLinkHeader.Length, link.Length - 1 - Movie.movieLinkHeader.Length);
                HtmlNode starNode = ddNode.SelectNodes("div[@class='star clearfix']")[0];
                rating = starNode.SelectNodes("span")[1].InnerText.Trim();
                if (rating == string.Empty)
                {
                    rating = "0";
                }
                star = Util.getStarPath(rating);
            }
            catch (Exception)
            {
                throw;
            }
            Movie m = new Movie();
            m.id = id;
            m.title = Util.replaceSpecialChar(name);
            m.rating = rating;
            m.star = star;
            m.posterUrl = posterUrl;
            return m;
        }


    }
}
