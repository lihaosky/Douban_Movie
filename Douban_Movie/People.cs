using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class People : INotifyPropertyChanged
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
        public static string DIRECTOR = "director";
        public static string ACTOR = "actor";

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
