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
using System.Windows.Controls.Primitives;

namespace PanoramaApp2
{
    public partial class MoviePage : PhoneApplicationPage
    {
        private MovieJsonParser movieParser;
        private bool shortReviewLoaded;
        private bool reviewLoaded;
        private bool imageLoaded;
        private Movie movie;
        private ShortReviewHtmlParser shortReviewParser = null;
        private ReviewParser reviewParser = null;
        private ImageHtmlParser imageParser = null;

        public MoviePage()
        {
            InitializeComponent();
            shortReviewLoaded = false;
            reviewLoaded = false;
            imageLoaded = false;
            movie = App.moviePassed;
            movieParser = new MovieJsonParser(movie);
            movieParser.progressBar = MovieProgressBar;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                MovieProgressBar.IsIndeterminate = true;
                MovieProgressBar.Visibility = System.Windows.Visibility.Visible;
                movieParser.getMovieByID();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (movieParser != null)
            {
                movieParser.cancelDownload();
            }
            if (shortReviewParser != null)
            {
                shortReviewParser.cancelDownload();
            }
            if (reviewParser != null)
            {
                reviewParser.cancelDownload();
            }
        }

        private void loadShortReview() 
        {
            loadMoreButton.IsEnabled = false;
            movie.nextShortReviewLink = Movie.movieLinkHeader + movie.id + "/comments";
            shortReviewParser = new ShortReviewHtmlParser(movie);
            shortReviewParser.progressBar = ShortReviewProgressBar;
            ShortReviewProgressBar.IsIndeterminate = true;
            ShortReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
            shortReviewParser.button = loadMoreButton;
            shortReviewParser.text = loadText;
            shortReviewSelector.ItemsSource = shortReviewParser.shortReviewCollection;
            shortReviewParser.parseShortReview();
        }

        private void loadReview()
        {
            loadMoreReviewButton.IsEnabled = false;
            movie.nextReviewLink = Movie.movieLinkHeader + movie.id + "/reviews";
            reviewParser = new ReviewParser(movie);
            reviewParser.progressBar = ReviewProgressBar;
            ReviewProgressBar.IsIndeterminate = true;
            ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
            reviewParser.button = loadMoreReviewButton;
            reviewParser.text = loadReviewText;
            reviewLongListSelector.ItemsSource = reviewParser.reviewCollection;
            reviewParser.parseReview();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((Pivot)sender).SelectedIndex;
            if (index == 2)
            {
                if (shortReviewLoaded == false)
                {
                    shortReviewLoaded = true;
                    loadShortReview();
                }
            }
            if (index == 3)
            {
                if (reviewLoaded == false)
                {
                    reviewLoaded = true;
                    loadReview();
                }
            }
            if (index == 4)
            {
                if (imageLoaded == false)
                {
                    imageLoaded = true;
                    loadImage();
                }
            }
        }

        private void loadImage()
        {
            loadMoreImageButton.IsEnabled = false;
            movie.nextImageLink = Movie.movieLinkHeader + movie.id + "/photos?type=S";
            imageParser = new ImageHtmlParser(movie);
            imageParser.progressBar = ImageProgressBar;
            ImageProgressBar.IsIndeterminate = true;
            ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
            imageParser.button = loadMoreImageButton;
            imageParser.text = loadImageText;
            imageListBox.ItemsSource = imageParser.imageCollection;
            imageParser.parseImage();
        }

        private void loadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (shortReviewParser != null)
            {
                loadMoreButton.IsEnabled = false;
                ShortReviewProgressBar.IsIndeterminate = true;
                ShortReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                shortReviewParser.parseShortReview();
            }
        }

        private void reviewLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void loadMoreReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (reviewParser != null)
            {
                loadMoreReviewButton.IsEnabled = false;
                ReviewProgressBar.IsIndeterminate = true;
                ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                reviewParser.parseReview();
            }
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void imageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageListBox != null && imageListBox.SelectedItem != null)
            {
                MovieImage image = (MovieImage)imageListBox.SelectedItem;
                App.imagePassed = image;
                App.imageCollectionPassed = imageParser.imageCollection;
                NavigationService.Navigate(new Uri("/ImagePage.xaml", UriKind.Relative));
                imageListBox.SelectedItem = null;
            }
        }

        private void loadMoreImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageParser != null)
            {
                loadMoreImageButton.IsEnabled = false;
                ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
                ImageProgressBar.IsIndeterminate = true;
                imageParser.parseImage();
            }
        }
    }
}