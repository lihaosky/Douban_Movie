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

namespace PanoramaApp2
{
    class Top250HtmlParser
    {
        public static Button loadMoreButton;
        public static TextBlock loadText;
        public static int currentIndex = 0;
        public static int maxIndex = 9;
        public static ObservableCollection<Movie> observableMovieList = new ObservableCollection<Movie>();

        public static void parseTop250()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += downloadTop250Completed;
            client.DownloadStringAsync(new Uri(Movie.top250 + "?start=" + currentIndex * 25 + "&format="));
            
        }

        public static void downloadTop250Completed(object sender, DownloadStringCompletedEventArgs e)
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
            foreach (Movie movie in movieList)
            {
                observableMovieList.Add(movie);
            }
            if (currentIndex > 0)
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                SystemTray.IsVisible = false;
            }
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
