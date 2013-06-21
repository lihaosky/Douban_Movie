using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PanoramaApp2.HtmlParser;
using System.Windows.Controls.Primitives;

namespace PanoramaApp2
{
    public partial class PeoplePage : PhoneApplicationPage
    {
        private People people = null;
        private bool movieLoaded = false;
        private bool imageLoaded = false;
        private Popup searchPopup;
        private PeopleHtmlParser peopleParser = null;
        private PeopleMovieHtmlParser peopleMovieParser = null;
        private PeopleImageHtmlParser peopleImageParser = null;

        public PeoplePage()
        {
            InitializeComponent();
            searchPopup = new Popup();
            people = App.peoplePassed;
            if (people != null)
            {
                People p = Cache.getPeople(people.id);
                if (p != null)
                {
                    people = p;
                }
                peopleParser = new PeopleHtmlParser(people);
                peopleParser.peopleGrid = PeopleGrid;
                peopleParser.peopleProgressBar = PeopleProgressBar;
                peopleParser.genderPanel = genderStackPanel;
                peopleParser.birthPanel = birthdayStackPanel;
                peopleParser.birthplacePanel = birthplaceStackPanel;
                peopleParser.constPanel = constStackPanel;
                peopleParser.occupationPanel = occupationStackPanel;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.fromTombStone)
            {
                NavigationService.Navigate(new Uri("/MainPage.xmal", UriKind.Relative));
            }
            else
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    if (peopleParser != null)
                    {
                        PeopleProgressBar.IsIndeterminate = true;
                        PeopleProgressBar.Visibility = System.Windows.Visibility.Visible;
                        peopleParser.parsePeople();
                    }
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (peopleMovieParser != null)
                {
                    peopleMovieParser.cancelDownload();
                }
                if (peopleImageParser != null)
                {
                    peopleImageParser.cancelDownload();
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((Pivot)sender).SelectedIndex;
            if (index == 1)
            {
                if (movieLoaded == false)
                {
                    movieLoaded = true;
                    loadMovie();
                }
            }
            if (index == 2)
            {
                if (imageLoaded == false)
                {
                    imageLoaded = true;
                    loadImage();
                }
            }
        }

        private void loadMovie()
        {
            if (people != null)
            {
                peopleMovieParser = new PeopleMovieHtmlParser(people);
                peopleMovieParser.progressBar = movieProgressBar;
                movieSelector.ItemsSource = peopleMovieParser.movieCollection;
                loadMoreMovieButton.IsEnabled = false;
                peopleMovieParser.button = loadMoreMovieButton;
                peopleMovieParser.text = loadMovieText;
                movieProgressBar.IsIndeterminate = true;
                movieProgressBar.Visibility = System.Windows.Visibility.Visible;
                peopleMovieParser.parseMovie();
            }
        }

        private void loadImage()
        {
            if (people != null)
            {
                loadMoreImageButton.IsEnabled = false;
                peopleImageParser = new PeopleImageHtmlParser(people);
                peopleImageParser.progressBar = ImageProgressBar;
                ImageProgressBar.IsIndeterminate = true;
                ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
                peopleImageParser.button = loadMoreImageButton;
                peopleImageParser.text = loadImageText;
                imageListBox.ItemsSource = peopleImageParser.imageCollection;
                peopleImageParser.parseImage();
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

        private void loadMoreImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (people != null && peopleImageParser != null)
            {
                loadMoreImageButton.IsEnabled = false;
                ImageProgressBar.Visibility = System.Windows.Visibility.Visible;
                ImageProgressBar.IsIndeterminate = true;
                peopleImageParser.loadMore();
            }
        }

        private void imageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imageListBox != null && imageListBox.SelectedItem != null)
            {
                MovieImage image = (MovieImage)imageListBox.SelectedItem;
                if (image != null)
                {
                    App.imagePassed = image;
                    App.imageCollectionPassed = peopleImageParser.imageCollection;
                    NavigationService.Navigate(new Uri("/ImagePage.xaml", UriKind.Relative));
                }
                imageListBox.SelectedItem = null;
            }
        }

        private void loadMoreMovieButton_Click(object sender, RoutedEventArgs e)
        {
            if (people != null && peopleMovieParser != null)
            {
                loadMoreMovieButton.IsEnabled = false;
                movieProgressBar.IsIndeterminate = true;
                movieProgressBar.Visibility = System.Windows.Visibility.Visible;
                peopleMovieParser.loadMore();
            }
        }

        private void movieSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (movieSelector != null && movieSelector.SelectedItem != null)
            {
                Movie m = (Movie)movieSelector.SelectedItem;
                if (m != null && m.id != string.Empty)
                {
                    App.moviePassed = m;
                    NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                }
                movieSelector.SelectedItem = null;
            }
        }
    }
}