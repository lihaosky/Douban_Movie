using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PanoramaApp2
{
    public class Review
    {
        private string _id = "";
        public string id 
        {
            get
            {
                return _id;
            }
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
        private string _review = "";
        public string review 
        {
            get
            {
                return _review;
            }
            set
            {
                _review = value;
                NotifyPropertyChanged("review");
            }
        }
        private string _reviewShort = "";
        public string reviewShort
        {
            get
            {
                return _reviewShort;
            }
            set
            {
                _reviewShort = value;
                NotifyPropertyChanged("reviewShort");
            }
        }
        private string _reviewer = "";
        public string reviewer
        {
            get
            {
                return _reviewer;
            }
            set
            {
                _reviewer = value;
                NotifyPropertyChanged("revewer");
            }
        }
        private string _date = "";
        public string date 
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                NotifyPropertyChanged("date");
            }
        }
        private string _movieId = "";
        public string movieId 
        {
            get
            {
                return _movieId;
            }
            set
            {
                _movieId = value;
                NotifyPropertyChanged("movieId");
            }
        }
        private string _movieName = "";
        public string movieName 
        {
            get
            {
                return _movieName;
            }
            set
            {
                _movieName = value;
                NotifyPropertyChanged("movieName");
            }
        }
        private string _star = "";
        public string star {
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

        private bool _hasMoreComment;
        public bool hasMoreComment
        {
            get
            {
                return _hasMoreComment;
            }
            set
            {
                _hasMoreComment = value;
            }
        }
        private string _nextCommentLink = "";
        public string nextCommentLink
        {
            get
            {
                return _nextCommentLink;
            }
            set
            {
                _nextCommentLink = value;
            }
        }
        public static string reviewLinkHeader = "http://movie.douban.com/review/";
        public static string hotReviewHeader = "http://movie.douban.com/review/best/";

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
