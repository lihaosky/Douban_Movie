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

            // Get hot movie
            HotMovieHtmlParser.selector = hotLongListSelector;
            HotMovieHtmlParser.popup = popup;
            HotMovieHtmlParser.parseHottMovie();

            // Get latest
            LatestHtmlParser.selector = latestListSelector;
            LatestHtmlParser.parseLatestMovie();

            // Get top 250 movie
            top250LongListSelector.ItemsSource = Top250HtmlParser.observableMovieList;
            Top250HtmlParser.loadText = loadText;
            Top250HtmlParser.loadMoreButton = loadMoreButton;
            Top250HtmlParser.parseTop250();

            // Get us box
            USBoxJsonParser.usboxLongListSelector = usboxLongListSelector;
            USBoxJsonParser.parseUSBox();

            // Create an application bar
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = "给我评分";
            ApplicationBar.MenuItems.Add(menuItem1);
            ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem();
            menuItem2.Text = "意见反馈";
            ApplicationBar.MenuItems.Add(menuItem2);
            ApplicationBarMenuItem menuItem3 = new ApplicationBarMenuItem();
            menuItem3.Text = "关于";
            ApplicationBar.MenuItems.Add(menuItem3);
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

        private void top250LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (top250LongListSelector != null && top250LongListSelector.SelectedItem != null)
            {
                Movie movie = (Movie)top250LongListSelector.SelectedItem;
                App.moviePassed = movie;
                NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                top250LongListSelector.SelectedItem = null;
            }
        }

        private void loadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            loadMoreButton.IsEnabled = false;
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            Top250HtmlParser.parseTop250();
        }

        private void latestListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (latestListSelector != null && latestListSelector.SelectedItem != null)
            {
                Movie movie = (Movie)latestListSelector.SelectedItem;
                App.moviePassed = movie;
                NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                latestListSelector.SelectedItem = null;
            }
        }
    }
}