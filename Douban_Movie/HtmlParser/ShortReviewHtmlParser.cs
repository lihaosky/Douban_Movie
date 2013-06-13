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

namespace PanoramaApp2
{
    class ShortReviewHtmlParser
    {
        public ObservableCollection<ShortReview> shortReviewCollection = new ObservableCollection<ShortReview>();
        public Button button { get; set; }
        public TextBlock text { get; set; }
        public ProgressBar progressBar { get; set; }
        private Movie movie { get; set; }
        private WebClient client;

        public ShortReviewHtmlParser(Movie m)
        {
            movie = m;
        }

        public void parseShortReview()
        {
            client = new WebClient();
            client.DownloadStringCompleted += downloadShortReviewCompleted;
            client.DownloadStringAsync(new Uri(movie.nextShortReviewLink));
        }

        public void downloadShortReviewCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='comment']");
                    if (nodeCollection == null)
                    {
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
                        movie.hasMoreShortReview = false;
                        button.IsEnabled = false;
                        text.Text = "完了:-)";
                    }
                    else
                    {
                        foreach (HtmlNode node in nodeCollection)
                        {
                            ShortReview sr;
                            try
                            {
                                sr = getShortReview(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            shortReviewCollection.Add(sr);
                        }
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
                        nodeCollection = doc.DocumentNode.SelectNodes("//div[@id='paginator']");
                        if (nodeCollection == null)
                        {
                            movie.hasMoreShortReview = false;
                            button.IsEnabled = false;
                            text.Text = "完了:-)";
                        }
                        else
                        {
                            HtmlNodeCollection nc = nodeCollection[0].SelectNodes("a[@class='next']");
                            if (nc == null)
                            {
                                movie.hasMoreShortReview = false;
                                button.IsEnabled = false;
                                text.Text = "完了:-)";
                            }
                            else
                            {
                                movie.hasMoreShortReview = true;
                                string link = nc[0].Attributes["href"].Value;
                                link = link.Replace("&amp;", "&");
                                movie.nextShortReviewLink = Movie.movieLinkHeader + movie.id + "/comments" + link;
                                button.IsEnabled = true;
                            }
                        }
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
            catch (WebException)
            {
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show("无法连接到豆瓣网,请检查网络连接", "", MessageBoxButton.OK);
            }
        }

        private ShortReview getShortReview(HtmlNode node)
        {
            string name = "";
            string time = "";
            string content = "";
            string star = App.imagePath + "00 star.png";
            try
            {
                HtmlNode spanNode = node.SelectNodes("h3")[0].SelectNodes("span[@class='comment-info']")[0];
                name = spanNode.SelectNodes("a")[0].InnerText;
                HtmlNodeCollection nodes = spanNode.SelectNodes("span");
                if (nodes.Count == 1)
                {
                    time = Util.replaceSpecialChar(nodes[0].InnerText.Trim());
                }
                else
                {
                    time = nodes[1].InnerText.Trim();
                    star = App.imagePath + nodes[0].Attributes["class"].Value.Substring(7, 2) + " star.png";
                }
                content = Util.formatShortReview(node.SelectNodes("p")[0].InnerText.Trim());
            }
            catch (Exception)
            {
                throw;
            }
            ShortReview sr = new ShortReview();
            sr.author = name;
            sr.time = time;
            sr.content = content;
            sr.star = star;
            return sr;
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
