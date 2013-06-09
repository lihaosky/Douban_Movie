using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    public class Movie : INotifyPropertyChanged
    {
        private string _id = "";
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("id");
            }
        }
        private string _title = "";
        public string title 
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("title");
            }
        }
        private string _posterUrl = "";
        public string posterUrl
        {
            get
            {
                return _posterUrl;
            }
            set
            {
                _posterUrl = value;
                NotifyPropertyChanged("posterUrl");
            }
        }
        private string _rating = "";
        public string rating {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                NotifyPropertyChanged("rating");
            }
        }
        private string _star = "";
        public string star
        {
            get
            {
                return _star;
            }
            set
            {
                _star = value;
                NotifyPropertyChanged("star");
            }
        }
        private string _actors_list = "";
        public string actors_list {
            get
            {
                return _actors_list;
            }
            set
            {
                _actors_list = value;
                NotifyPropertyChanged("actors_list");
            }
        }
        private string _length = "";
        public string length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                NotifyPropertyChanged("length");
            }
        }
        private string _year = "";
        public string year 
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                NotifyPropertyChanged("year");
            }
        }
        private string _region = "";
        public string region 
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
                NotifyPropertyChanged("region");
            }
        }
        private string _rateNumber = "";
        public string rateNumber
        {
            get
            {
                return _rateNumber;
            }
            set
            {
                _rateNumber = value;
                NotifyPropertyChanged("rateNumber");
            }
        }
        private string _summary = "";
        public string summary {
            get
            {
                return _summary;
            }
            set
            {
                _summary = value;
                NotifyPropertyChanged("summary");
            }
        }
        private string _genre = "";
        public string genre 
        {
            get
            {
                return _genre;
            }
            set
            {
                _genre = value;
                NotifyPropertyChanged("genre");
            }
        }
        private string _money = "";
        public string money 
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
                NotifyPropertyChanged("money");
            }
        }
        private string _quote = "";
        public string quote 
        {
            get
            {
                return _quote;
            }
            set
            {
                _quote = value;
                NotifyPropertyChanged("quote");
            }
        }
        private string _releaseDate = "";
        public string releaseDate 
        {
            get
            {
                return _releaseDate;
            }
            set
            {
                _releaseDate = value;
                NotifyPropertyChanged("releaseDate");
            }
        }
        public static string movieLinkHeader = "http://movie.douban.com/subject/";
        public static string apiMovieHeader = "http://api.douban.com/v2/movie/subject/";
        public static string apiUSBoxHeader = "http://api.douban.com/v2/movie/us_box";
        public static string homePage = "http://movie.douban.com";
        public static string top250 = "http://movie.douban.com/top250";
        public static string latest = "http://movie.douban.com/later/beijing/";

        public event PropertyChangedEventHandler PropertyChanged;
        // NotifyPropertyChanged will raise the PropertyChanged event, 
        // passing the source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
