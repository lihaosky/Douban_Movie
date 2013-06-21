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
using PanoramaApp2.HtmlParser;

namespace PanoramaApp2
{
    public partial class MoviePage : PhoneApplicationPage
    {
        private MovieJsonParser movieParser = null;
        private Popup searchPopup;
        private bool shortReviewLoaded;
        private bool reviewLoaded;
        private bool imageLoaded;
        private Movie movie = null;
        private ShortReviewHtmlParser shortReviewParser = null;
        private ReviewParser reviewParser = null;
        private ImageHtmlParser imageParser = null;

        public MoviePage()
        {
            InitializeComponent();
            searchPopup = new Popup();
            shortReviewLoaded = false;
            reviewLoaded = false;
            imageLoaded = false;
            movie = App.moviePassed;
            if (movie != null)
            {
                Movie result = Cache.getMovie(movie.id);
                if (result != null)
                {
                    movie = result;
                }
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.fromTombStone)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    if (movieParser != null)
                    {
                        MovieProgressBar.IsIndeterminate = true;
                        MovieProgressBar.Visibility = System.Windows.Visibility.Visible;
                        movieParser.getMovieByID();
                    }
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (searchPopup.IsOpen)
            {
                searchPopup.IsOpen = false;
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
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
                if (imageParser != null)
                {
                    imageParser.cancelDownload();
                }
            }
            base.OnNavigatedFrom(e);
        }

        private void loadShortReview() 
        {
            if (movie != null)
            {
                loadMoreButton.IsEnabled = false;
                shortReviewParser = new ShortReviewHtmlParser(movie);
                shortReviewParser.progressBar = ShortReviewProgressBar;
                ShortReviewProgressBar.IsIndeterminate = true;
                ShortReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                shortReviewParser.button = loadMoreButton;
                shortReviewParser.text = loadText;
                shortReviewSelector.ItemsSource = shortReviewParser.shortReviewCollection;
                shortReviewParser.parseShortReview();
            }
        }

        private void loadReview()
        {
            if (movie != null)
            {
                loadMoreReviewButton.IsEnabled = false;
                reviewParser = new ReviewParser(movie);
                reviewParser.progressBar = ReviewProgressBar;
                reviewParser.button = loadMoreReviewButton;
                reviewParser.text = loadReviewText;
                reviewLongListSelector.ItemsSource = reviewParser.reviewCollection;
                ReviewProgressBar.IsIndeterminate = true;
                ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                reviewParser.parseReview();
            }
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
            if (movie != null)
            {
                loadMoreImageButton.IsEnabled = false;
                imageParser = new ImageHtmlParser(movie);
                imageParser.progressBar = ImageProgressBar;
                ImageProgressBar.IsIndeterminate = true;
                ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
                imageParser.button = loadMoreImageButton;
                imageParser.text = loadImageText;
                imageListBox.ItemsSource = imageParser.imageCollection;
                imageParser.parseImage();
            }
        }

        private void loadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (shortReviewParser != null && movie != null)
            {
                loadMoreButton.IsEnabled = false;
                ShortReviewProgressBar.IsIndeterminate = true;
                ShortReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                shortReviewParser.loadMore();
            }
        }

        private void reviewLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reviewLongListSelector != null && reviewLongListSelector.SelectedItem != null) {
                Review review = (Review)reviewLongListSelector.SelectedItem;
                if (review != null)
                {
                    App.reviewPassed = review;
                    NavigationService.Navigate(new Uri("/ReviewPage.xaml", UriKind.Relative));
                }
                reviewLongListSelector.SelectedItem = null;
            }
        }

        private void loadMoreReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (reviewParser != null && movie != null)
            {
                loadMoreReviewButton.IsEnabled = false;
                ReviewProgressBar.IsIndeterminate = true;
                ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                reviewParser.loadMore();
            }
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PopupInput input = new PopupInput();
            input.Width = Application.Current.Host.Content.ActualWidth;
            input.Height = Application.Current.Host.Content.ActualHeight;
            searchPopup.Child = input;
            searchPopup.IsOpen = true;
            input.inputBox.Focus();
        }

        private void imageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageListBox != null && imageListBox.SelectedItem != null)
            {
                MovieImage image = (MovieImage)imageListBox.SelectedItem;
                if (image != null)
                {
                    App.imagePassed = image;
                    App.imageCollectionPassed = imageParser.imageCollection;
                    NavigationService.Navigate(new Uri("/ImagePage.xaml", UriKind.Relative));
                }
                imageListBox.SelectedItem = null;
            }
        }

        private void loadMoreImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageParser != null && movie != null)
            {
                loadMoreImageButton.IsEnabled = false;
                ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
                ImageProgressBar.IsIndeterminate = true;
                imageParser.loadMore();
            }
        }

        private void peopleSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (peopleSelector != null && peopleSelector.SelectedItem != null)
            {
                People people = (People)peopleSelector.SelectedItem;
                if (people != null && people.id != null && people.id != "")
                {
                    App.peoplePassed = people;
                    NavigationService.Navigate(new Uri("/PeoplePage.xaml", UriKind.Relative));
                }
                peopleSelector.SelectedItem = null;
            }
        }
    }
}