using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    public class ShortReview : INotifyPropertyChanged
    {
        private string _author = "";
        public string author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                NotifyPropertyChanged("author");
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
        private string _time = "";
        public string time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                NotifyPropertyChanged("time");
            }
        }
        private string _content = "";
        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                NotifyPropertyChanged("content");
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is ShortReview)
            {
                ShortReview tmp = obj as ShortReview;
                return tmp.content == this.content;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.content.GetHashCode();
        }
    }
}
