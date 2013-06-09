using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace PanoramaApp2
{
    public partial class MoviePage : PhoneApplicationPage
    {
        private MovieJsonParser movieParser;

        public MoviePage()
        {
            InitializeComponent();
            this.Loaded += page_loaded;
        }

        void page_loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            SystemTray.SetForegroundColor(this, System.Windows.Media.Colors.White);
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            Movie movie = App.moviePassed;
            if (movie == null)
            {
                System.Diagnostics.Debug.WriteLine("Shouldn't be null!");
            }
            movieParser = new MovieJsonParser();
            movieParser.movie = movie;
            movieParser.title = title;
            movieParser.posterImage = posterUrl;
            movieParser.rating = rating;
            movieParser.rateNumber = rateNumber;
            movieParser.year_duration = year_duration;
            movieParser.starImage = ratingUrl;
            movieParser.name = fixedName;
            movieParser.region = region;
            movieParser.genre = genre;
            movieParser.trailer = trailer;
            movieParser.theater = theater;
            movieParser.summary = summary;
            movieParser.peopleList = peopleSelector;
            movieParser.getMovieByID();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (movieParser != null)
            {
                movieParser.cancelDownLoad();
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}