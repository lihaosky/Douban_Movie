using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class Movie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string posterUrl { get; set; }
        public string rating { get; set; }
        public string star { get; set; }
        public string actors_list { get; set; }
        public static string movieLinkHeader = "http://movie.douban.com/subject/";
    }
}
