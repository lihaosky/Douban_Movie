using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PanoramaApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            Movie m1 = new Movie
            {
                movieID = "11529526",
                movieName = "中国合伙人",
                imageUrl = "http://img3.douban.com/view/movie_poster_cover/mpst/public/p1959304567.jpg"
            };

            Movie m2 = new Movie
            {
                movieID = "11529526",
                movieName = "中国合伙人",
                imageUrl = "http://img3.douban.com/view/movie_poster_cover/mpst/public/p1959304567.jpg"
            };

            Movie m3 = new Movie
            {
                movieID = "11529526",
                movieName = "中国合伙人",
                imageUrl = "http://img3.douban.com/view/movie_poster_cover/mpst/public/p1959304567.jpg"
            };

            List<Movie> movies = new List<Movie>();
            movies.Add(m1);
            movies.Add(m2);
            movies.Add(m3);
            hotLongListSelector.ItemsSource = movies;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}