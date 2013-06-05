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
        public MoviePage()
        {
            InitializeComponent();
            Movie movie = App.moviePassed;
            if (movie == null)
            {
                System.Diagnostics.Debug.WriteLine("Shouldn't be null!");
            }
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            JsonParser jparser = new JsonParser();
            jparser.progressbar = progressBar;
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

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}