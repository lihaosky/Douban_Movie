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
    class ReviewParser
    {
        private Movie movie;
        public Button button { get; set; }
        public TextBlock text { get; set; }
        public ProgressBar progressBar { get; set; }
        public ObservableCollection<Review> reviewCollection = new ObservableCollection<Review>();
        private WebClient client;
        private bool isFromLoadMore = false;

        public ReviewParser(Movie m)
        {
            movie = m;
        }

        public void parseReview()
        {
            if (movie.reviewLoaded)
            {
                foreach (Review r in movie.reviewSet)
                {
                    reviewCollection.Add(r);
                }
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                if (movie.hasMoreReview == false)
                {
                    button.IsEnabled = false;
                    text.Text = AppResources.Finish;
                }
                else
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                movie.nextReviewLink = Movie.movieLinkHeader + movie.id + "/reviews";
                client = new WebClient();
                client.DownloadStringCompleted += downloadReviewCompleted;
                client.DownloadStringAsync(new Uri(movie.nextReviewLink));
            }
        }

        public void loadMore()
        {
            isFromLoadMore = true;
            client = new WebClient();
            client.DownloadStringCompleted += downloadReviewCompleted;
            client.DownloadStringAsync(new Uri(movie.nextReviewLink));
        }

        public void downloadReviewCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='review']");
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
                            Review r;
                            try
                            {
                                r = getReview(node);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            reviewCollection.Add(r);
                            movie.reviewSet.Add(r);
                        }
                        if (progressBar != null)
                        {
                            progressBar.Visibility = Visibility.Collapsed;
                        }
                        nodeCollection = doc.DocumentNode.SelectNodes("//div[@id='paginator']");
                        if (nodeCollection == null)
                        {
                            movie.hasMoreReview = false;
                            button.IsEnabled = false;
                            text.Text = AppResources.Finish;
                        }
                        else
                        {
                            HtmlNodeCollection nc = nodeCollection[0].SelectNodes("a[@class='next']");
                            if (nc == null)
                            {
                                movie.hasMoreReview = false;
                                button.IsEnabled = false;
                                text.Text = AppResources.Finish;
                            }
                            else
                            {
                                movie.hasMoreReview = true;
                                string link = nc[0].Attributes["href"].Value;
                                link = link.Replace("&amp;", "&");
                                movie.nextReviewLink = Movie.movieLinkHeader + movie.id + "/reviews" + link;
                                button.IsEnabled = true;
                            }
                        }
                    }
                    movie.reviewLoaded = true;
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            if (isFromLoadMore)
                            {
                                loadMore();
                            }
                            else
                            {
                                parseReview();
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
                button.IsEnabled = true;
                if (progressBar != null)
                {
                    progressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                button.IsEnabled = true;
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

        private Review getReview(HtmlNode node)
        {
            string author = "";
            string star = "";
            string date = "";
            string shortReview = "";
            string title = "";
            string id = "";

            try
            {
                HtmlNode titleNode = node.SelectNodes("div[@class='review-hd']")[0].SelectNodes("h3")[0].SelectNodes("a")[1];
                title = Util.replaceSpecialChar(titleNode.InnerText.Trim());
                string link = titleNode.Attributes["href"].Value;
                id = link.Substring(Review.reviewLinkHeader.Length, link.Length - 1 - Review.reviewLinkHeader.Length);
                HtmlNode infoNode = node.SelectNodes("div[@class='review-hd']")[0].SelectNodes("div[@class='review-hd-info']")[0];
                HtmlNode aNode = infoNode.SelectNodes("a")[0];
                HtmlNode spanNode = infoNode.SelectNodes("span")[0];
                author = Util.replaceSpecialChar(aNode.InnerText.Trim());
                star = App.imagePath + spanNode.Attributes["class"].Value.Substring(7, 2) + " star.png";
                aNode.Remove();
                spanNode.Remove();
                date = infoNode.InnerText.Trim();
                shortReview = Util.formatShortReview(node.SelectNodes("div[@class='review-bd']")[0].SelectNodes("div[@class='review-short']")[0].SelectNodes("span[@class='']")[0].InnerText.Trim());
            }
            catch (Exception)
            {
                throw;
            }
            Review r = new Review();
            r.date = date;
            r.id = id;
            r.movieName = movie.title;
            r.movieId = movie.id;
            r.reviewer = author;
            r.reviewShort = shortReview;
            r.star = star;
            r.title = title;
            return r;
        }
    }
}
