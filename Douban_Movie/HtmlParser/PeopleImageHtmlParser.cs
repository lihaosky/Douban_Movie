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
    class PeopleImageHtmlParser
    {
        private People people;
        public ProgressBar progressBar { get; set; }
        public Button button;
        public TextBlock text;
        public ObservableCollection<MovieImage> imageCollection = new ObservableCollection<MovieImage>();
        private WebClient client;
        private bool isFromLoadMore = false;

        public PeopleImageHtmlParser(People p)
        {
            people = p;
        }

        public void parseImage()
        {
            if (people.imageLoaded == false)
            {
                people.nextImageLink = People.peopleLinkHeader + people.id + "/photos";
                client = new WebClient();
                client.DownloadStringCompleted += downloadImageCompleted;
                client.DownloadStringAsync(new Uri(people.nextImageLink));
            }
            else
            {
                foreach (MovieImage m in people.imageSet)
                {
                    imageCollection.Add(m);
                }
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                if (people.hasNextImage)
                {
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = false;
                    text.Text = AppResources.Finish;
                }
            }
        }

        public void loadMore()
        {
            isFromLoadMore = true;
            client = new WebClient();
            client.DownloadStringCompleted += downloadImageCompleted;
            client.DownloadStringAsync(new Uri(people.nextImageLink));
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
                    HtmlNodeCollection uCollection = doc.DocumentNode.SelectNodes("//ul[@class='poster-col4 clearfix']");
                    if (uCollection == null)
                    {
                        people.hasNextImage = false;
                        button.IsEnabled = false;
                        text.Text = AppResources.Finish;
                    }
                    else
                    {
                        HtmlNodeCollection liCollection = uCollection[0].SelectNodes("li");
                        if (liCollection == null)
                        {
                            people.hasNextImage = false;
                            button.IsEnabled = false;
                            text.Text = AppResources.Finish;
                        }
                        else
                        {
                            foreach (HtmlNode node in liCollection)
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
                                people.imageSet.Add(image);
                            }
                            HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='paginator']");
                            if (nodeCollection == null)
                            {
                                people.hasNextImage = false;
                                button.IsEnabled = false;
                                text.Text = AppResources.Finish;
                            }
                            else
                            {
                                HtmlNodeCollection nc = nodeCollection[0].SelectNodes("span[@class='next']");
                                if (nc == null)
                                {
                                    people.hasNextImage = false;
                                    button.IsEnabled = false;
                                    text.Text = AppResources.Finish;
                                }
                                else
                                {
                                    HtmlNodeCollection aCollection = nc[0].SelectNodes("a");
                                    if (aCollection == null)
                                    {
                                        people.hasNextImage = false;
                                        button.IsEnabled = false;
                                        text.Text = AppResources.Finish;
                                    }
                                    else
                                    {
                                        people.hasNextImage = true;
                                        string link = aCollection[0].Attributes["href"].Value.Trim();
                                        people.nextImageLink = link;
                                        button.IsEnabled = true;
                                    }
                                }
                            }
                        }
                    }
                    people.imageLoaded = true;
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
                        System.Diagnostics.Debug.WriteLine("canceled");
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            if (isFromLoadMore)
                            {
                                isFromLoadMore = false;
                                loadMore();
                            }
                            else
                            {
                                parseImage();
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
            }
            catch (WebException)
            {
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
            catch (Exception)
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

        private MovieImage getImage(HtmlNode node)
        {
            string smallUrl = "";
            string largeUrl = "";
            string id = "";
            try
            {
                string link = node.SelectNodes("div[@class='cover']")[0].SelectNodes("a")[0].Attributes["href"].Value.Trim();
                int headerLength = people.id.Length + 7;
                id = link.Substring(People.peopleLinkHeader.Length + headerLength, link.Length - headerLength - 1 - People.peopleLinkHeader.Length);
                smallUrl = MovieImage.smallUrlHeader + id + ".jpg";
                largeUrl = MovieImage.largeUrlHeader + id + ".jpg";
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
