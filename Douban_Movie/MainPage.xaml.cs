using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;

namespace PanoramaApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            showPopup();
            HotMovieHtmlParser parser = new HotMovieHtmlParser();
            parser.selector = hotLongListSelector;
            parser.popup = popup;
            parser.parseHottMovie();
            USBoxJsonParser usBoxParser = new USBoxJsonParser();
            usBoxParser.usboxLongListSelector = usboxLongListSelector;
            usBoxParser.parseUSBox();
        }

        private void showPopup()
        {
            popup = new Popup();
            PopupSplash splash = new PopupSplash();
            splash.Height = Application.Current.Host.Content.ActualHeight;
            splash.Width = Application.Current.Host.Content.ActualWidth;
            popup.Child = splash;
            popup.IsOpen = true;
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

        private void hotLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hotLongListSelector != null && hotLongListSelector.SelectedItem != null)
            {
                Movie movie = (Movie)hotLongListSelector.SelectedItem;
                App.moviePassed = movie;
                NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                hotLongListSelector.SelectedItem = null;
            }
        }

        private void usboxLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usboxLongListSelector != null && usboxLongListSelector.SelectedItem != null)
            {
                Movie movie = (Movie)usboxLongListSelector.SelectedItem;
                App.moviePassed = movie;
                NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                usboxLongListSelector.SelectedItem = null;
            }
        }
    }
}