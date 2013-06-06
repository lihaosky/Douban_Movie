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
        private MovieJsonParser jparser;

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
            jparser = new MovieJsonParser();
            jparser.movie = movie;
            jparser.title = title;
            jparser.posterImage = posterUrl;
            jparser.rating = rating;
            jparser.rateNumber = rateNumber;
            jparser.year_duration = year_duration;
            jparser.starImage = ratingUrl;
            jparser.name = fixedName;
            jparser.region = region;
            jparser.genre = genre;
            jparser.trailer = trailer;
            jparser.theater = theater;
            jparser.summary = summary;
            jparser.getMovieByID();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (jparser != null)
            {
                jparser.cancelDownLoad();
            }
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}