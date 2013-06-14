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
    class ImageHtmlParser
    {
        private Movie movie;
        public Button button { get; set; }
        public TextBlock text { get; set; }
        public ProgressBar progressBar { get; set; }
        public ObservableCollection<MovieImage> imageCollection = new ObservableCollection<MovieImage>();
        private WebClient client;

        public ImageHtmlParser(Movie m)
        {
            movie = m;
        }

        public void parseImage()
        {
            client = new WebClient();
            client.DownloadStringCompleted += downloadImageCompleted;
            client.DownloadStringAsync(new Uri(movie.nextImageLink));
        }

        public void downloadImageCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='cover']");
                    if (nodeCollection == null)
                    {
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
                        movie.hasMoreShortReview = false;
                        button.IsEnabled = false;
                        text.Text = AppResources.Finish;
                    }
                    else
                    {
                        foreach (HtmlNode node in nodeCollection)
                        {
                            MovieImage image;
                            try
                            {
                                image = getImage(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            imageCollection.Add(image);
                        }
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
                        nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='paginator']");
                        if (nodeCollection == null)
                        {
                            movie.hasMoreImage = false;
                            button.IsEnabled = false;
                            text.Text = AppResources.Finish;
                        }
                        else
                        {
                            HtmlNodeCollection nc = nodeCollection[0].SelectNodes("span[@class='next']");
                            if (nc == null)
                            {
                                movie.hasMoreImage = false;
                                button.IsEnabled = false;
                                text.Text = AppResources.Finish;
                            }
                            else
                            {
                                HtmlNodeCollection aCollection = nc[0].SelectNodes("a");
                                if (aCollection == null)
                                {
                                    movie.hasMoreImage = false;
                                    button.IsEnabled = false;
                                    text.Text = AppResources.Finish;
                                }
                                else
                                {
                                    movie.hasMoreImage = true;
                                    string link = aCollection[0].Attributes["href"].Value;
                                    link = link.Replace("&amp;", "&");
                                    movie.nextImageLink = link;
                                    button.IsEnabled = true;
                                }
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
                button.IsEnabled = true;
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
        }

        private MovieImage getImage(HtmlNode node)
        {
            string smallUrl = "";
            string largeUrl = "";
            string id = "";
            try
            {
                string link = node.SelectNodes("a")[0].Attributes["href"].Value.Trim();
                id = link.Substring(Movie.homePage.Length + 14, link.Length - 15 - Movie.homePage.Length);
                smallUrl = "http://img3.douban.com/view/photo/thumb/public/p" + id + ".jpg";
                largeUrl = "http://img3.douban.com/view/photo/raw/public/p" + id + ".jpg";
            }
            catch (Exception)
            {
                throw;
            }
            MovieImage image = new MovieImage();
            image.id = id;
            image.smallUrl = smallUrl;
            image.largeUrl = largeUrl;
            return image;
        }
    }
}
