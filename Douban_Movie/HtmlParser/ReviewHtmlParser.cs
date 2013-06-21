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
    class ReviewHtmlParser
    {
        private Review review;
        public ProgressBar reviewProgressBar { set; get; }
        public ProgressBar commentProgressBar { set; get; }
        public StackPanel reviewStackPanel { get; set; }
        public Button button { get; set; }
        public Border border { get; set; }
        public TextBlock movieText { get; set; }
        public TextBlock text { get; set; }
        private WebClient client;
        public ObservableCollection<Comment> commentCollection = new ObservableCollection<Comment>();

        public ReviewHtmlParser(Review r)
        {
            review = r;
        }

        public void parseReview()
        {
            Review r = Cache.getReview(review.id);
            if (r == null)
            {
                client = new WebClient();
                client.DownloadStringCompleted += downloadReviewCompleted;
                client.DownloadStringAsync(new Uri(Review.reviewLinkHeader + review.id));
            }
            else
            {
                border.Visibility = Visibility.Visible;
                movieText.Visibility = Visibility.Visible;
                reviewStackPanel.DataContext = r;
                foreach (Comment c in review.commentSet)
                {
                    commentCollection.Add(c);
                }
                if (r.hasMoreComment == false)
                {
                    button.IsEnabled = false;
                    text.Text = AppResources.Finish;
                }
                else
                {
                    button.IsEnabled = true;
                }
                if (reviewProgressBar != null)
                {
                    reviewProgressBar.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void parseComment()
        {
            client = new WebClient();
            client.DownloadStringCompleted += downloadCommentCompleted;
            client.DownloadStringAsync(new Uri(review.nextCommentLink));
        }

        private void downloadCommentCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    try
                    {
                        loadComments(doc);
                    }
                    catch (Exception)
                    {
                    }
                    commentProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            parseComment();
                        }
                    }
                    else
                    {
                        commentProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (WebException)
            {
                button.IsEnabled = true;
                if (reviewProgressBar != null)
                {
                    reviewProgressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
        }

        public void cancelDownload()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }

        private void downloadReviewCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled)
                {
                    string page = e.Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);
                    try
                    {
                        getReview(doc);
                        border.Visibility = Visibility.Visible;
                        movieText.Visibility = Visibility.Visible;
                        reviewStackPanel.DataContext = review;
                        Cache.insertReview(review);
                    }
                    catch (Exception)
                    {
                    }
                    if (reviewProgressBar != null)
                    {
                        reviewProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    var wEx = e.Error as WebException;
                    if (wEx.Status == WebExceptionStatus.RequestCanceled)
                    {
                        if (App.isFromDormant)
                        {
                            App.isFromDormant = false;
                            parseReview();
                        }
                    }
                    if (reviewProgressBar != null)
                    {
                        reviewProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (WebException)
            {
                if (reviewProgressBar != null)
                {
                    reviewProgressBar.Visibility = Visibility.Collapsed;
                }
                MessageBoxResult result = MessageBox.Show(AppResources.ConnectionError, "", MessageBoxButton.OK);
            }
        }

        private Comment getComment(HtmlNode node)
        {
            string author = "";
            string time = "";
            string content = "";

            try
            {
                HtmlNode commentNode = node.SelectNodes("div[@class='content report-comment']")[0];
                HtmlNode authorNode = commentNode.SelectNodes("div[@class='author']")[0];
                HtmlNode aNode = authorNode.SelectNodes("a")[0];
                author = Util.replaceSpecialChar(aNode.InnerText.Trim());
                aNode.Remove();
                time = authorNode.InnerText.Trim().Substring(0, 19);
                content = Util.formatReview(commentNode.SelectNodes("p")[0].InnerText);
            }
            catch (Exception)
            {
                throw;
            }
            Comment c = new Comment();
            c.author = author;
            c.time = time;
            c.content = content;
            return c;
        }

        private void loadComments(HtmlDocument doc)
        {
            try
            {
                HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//div[@class='comment-item']");
                if (collection == null)
                {
                    review.hasMoreComment = false;
                    button.IsEnabled = false;
                    text.Text = AppResources.Finish;
                }
                else
                {
                    foreach (HtmlNode node in collection)
                    {
                        Comment c;
                        try
                        {
                            c = getComment(node);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        commentCollection.Add(c);
                        review.commentSet.Add(c);
                    }
                    HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='paginator']");
                    if (nodeCollection == null)
                    {
                        review.hasMoreComment = false;
                        button.IsEnabled = false;
                        text.Text = AppResources.Finish;
                    }
                    else
                    {
                        HtmlNodeCollection nc = nodeCollection[0].SelectNodes("span[@class='next']");
                        if (nc == null)
                        {
                            review.hasMoreComment = false;
                            button.IsEnabled = false;
                            text.Text = AppResources.Finish;
                        }
                        else
                        {
                            HtmlNodeCollection aCollection = nc[0].SelectNodes("a");
                            if (aCollection == null)
                            {
                                review.hasMoreComment = false;
                                button.IsEnabled = false;
                                text.Text = AppResources.Finish;
                            }
                            else
                            {
                                review.hasMoreComment = true;
                                string link = aCollection[0].Attributes["href"].Value;
                                review.nextCommentLink = link;
                                button.IsEnabled = true;
                            }
                        }
                    }
                }
                review.commentLoaded = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void getReview(HtmlDocument doc)
        {
            try
            {
                string date = doc.DocumentNode.SelectNodes("//span[@property='v:dtreviewed']")[0].InnerText.Trim();
                if (review.date == null || review.date == "")
                {
                    review.date = date;
                }

                HtmlNode reviewNode = doc.DocumentNode.SelectNodes("//div[@property='v:description']")[0];
                HtmlNodeCollection aNodeCollection = reviewNode.SelectNodes("a");
                if (aNodeCollection != null)
                {
                    foreach (HtmlNode node in aNodeCollection)
                    {
                        HtmlNode newNode = HtmlNode.CreateNode(node.InnerText);
                        reviewNode.ReplaceChild(newNode, node);
                    }
                }
                string r = reviewNode.InnerHtml;
                review.review = Util.formatReview(r);
                loadComments(doc);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
