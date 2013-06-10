using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class MovieImage : INotifyPropertyChanged
    {
        public MovieImage prev = null;
        public MovieImage next = null;
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

        private string _smallUrl = "";
        public string smallUrl
        {
            get
            {
                return _smallUrl;
            }
            set
            {
                _smallUrl = value;
                NotifyPropertyChanged("smallUrl");
            }
        }

        private string _largeUrl = "";
        public string largeUrl
        {
            get
            {
                return _largeUrl;
            }
            set
            {
                _largeUrl = value;
                NotifyPropertyChanged("largeUrl");
            }
        }
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
