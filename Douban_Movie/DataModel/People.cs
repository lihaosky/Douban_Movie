using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    public class People : INotifyPropertyChanged
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
        private string _name = "";
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("name");
            }
        }
        private string _postion = "";
        public string position
        {
            get
            {
                return _postion;
            }
            set
            {
                _postion = value;
                NotifyPropertyChanged("position");
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
        private string _positionName = "";
        public string positionName
        {
            get
            {
                return _positionName;
            }
            set
            {
                _positionName = value;
                NotifyPropertyChanged("positionName");
            }
        }
        private string _gender = "";
        public string gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                NotifyPropertyChanged("gender");
            }
        }
        private string _constl = "";
        public string constl
        {
            get
            {
                return _constl;
            }
            set
            {
                _constl = value;
                NotifyPropertyChanged("constl");
            }
        }
        private string _birthday = "";
        public string birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
                NotifyPropertyChanged("birthday");
            }
        }
        private string _birthplace = "";
        public string birthplace
        {
            get
            {
                return _birthplace;
            }
            set
            {
                _birthplace = value;
                NotifyPropertyChanged("birthplace");
            }
        }
        private string _occupation = "";
        public string occupation
        {
            get
            {
                return _occupation;
            }
            set
            {
                _occupation = value;
                NotifyPropertyChanged("birthplace");
            }
        }
        private string _summary = "";
        public string summary
        {
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
        private bool _hasMoreMovie;
        public bool hasMoreMovie
        {
            get
            {
                return _hasMoreMovie;
            }
            set
            {
                _hasMoreMovie = value;
            }
        }
        private string _nextMovieLink = "";
        public string nextMovieLink
        {
            get
            {
                return _nextMovieLink;
            }
            set
            {
                _nextMovieLink = value;
            }
        }
        private bool _hasNextImage;
        public bool hasNextImage
        {
            get
            {
                return _hasNextImage;
            }
            set
            {
                _hasNextImage = value;
            }
        }
        private string _nextImageLink = "";
        public string nextImageLink
        {
            get
            {
                return _nextImageLink;
            }
            set
            {
                _nextImageLink = value;
            }
        }
        private HashSet<Movie> _movieSet = new HashSet<Movie>();
        public HashSet<Movie> movieSet
        {
            get
            {
                return _movieSet;
            }
            set
            {
                _movieSet = value;
            }
        }
        private bool _movieLoaded = false;
        public bool movieLoaded
        {
            get
            {
                return _movieLoaded;
            }
            set
            {
                _movieLoaded = value;
            }
        }
        private HashSet<MovieImage> _imageSet = new HashSet<MovieImage>();
        public HashSet<MovieImage> imageSet
        {
            get
            {
                return _imageSet;
            }
            set
            {
                _imageSet = value;
            }
        }
        private bool _imageLoaded = false;
        public bool imageLoaded
        {
            get
            {
                return _imageLoaded;
            }
            set
            {
                _imageLoaded = value;
            }
        }

        public static string DIRECTOR = "director";
        public static string ACTOR = "actor";
        public static string peopleLinkHeader = "http://movie.douban.com/celebrity/";

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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is People)
            {
                People tmp = obj as People;
                return tmp.id == this.id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
