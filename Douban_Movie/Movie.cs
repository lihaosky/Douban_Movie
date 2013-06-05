using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    public class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string posterUrl { get; set; }
        public string rating { get; set; }
        public string star { get; set; }
        public string actors_list { get; set; }
        public string length { get; set; }
        public string year { get; set; }
        public string region { get; set; }
        public string rateNumber { get; set; }
        public string summary { get; set; }
        public string genre { get; set; }
        public string money { get; set; }
        public string quote { get; set; }
        public static string movieLinkHeader = "http://movie.douban.com/subject/";
        public static string apiMovieHeader = "http://api.douban.com/v2/movie/subject/";
        public static string apiUSBoxHeader = "http://api.douban.com/v2/movie/us_box";
        public static string homePage = "http://movie.douban.com";
        public static string top250 = "http://movie.douban.com/top250";
    }
}
