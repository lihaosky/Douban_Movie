using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class Review
    {
        public string id { get; set; }
        public string title { get; set; }
        public string review { get; set; }
        public string reviewer { get; set; }
        public string date { get; set; }
        public string movieId { get; set; }
        public string movieName { get; set; }
        public string star { get; set; }

        public static string reviewLinkHeader = "http://movie.douban.com/review/";
        public static string hotReviewHeader = "http://movie.douban.com/review/best/";
    }
}
