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
        private bool latestLoaded;
        private bool top250Loaded;
        private bool usboxLoaded;
        private bool commentLoaded;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            latestLoaded = false;
            top250Loaded = false;
            usboxLoaded = false;
            commentLoaded = false;

            // Get hot movie
            HotMovieHtmlParser.selector = hotLongListSelector;
            popup = new Popup();
            HotMovieHtmlParser.popup = popup;
            
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
            
            PopupSplash splash = new PopupSplash();
            splash.Height = Application.Current.Host.Content.ActualHeight;
            splash.Width = Application.Current.Host.Content.ActualWidth;
            popup.Child = splash;
            popup.IsOpen = true;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                showPopup();
                HotMovieHtmlParser.parseHottMovie();
            }
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
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            Top250HtmlParser.indicator = indicator;
            Top250HtmlParser.parseTop250();
        }

        private void loadReviewMoreButton_Click(object sender, RoutedEventArgs e)
        {
            loadMoreReviewButton.IsEnabled = false;
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            HotReviewHtmlParser.indicator = indicator;
            HotReviewHtmlParser.parseHotReview();
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

        private void hotReviewLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void loadLatestPivotItem()
        {
            // Get latest
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            LatestHtmlParser.selector = latestListSelector;
            LatestHtmlParser.indicator = indicator;
            LatestHtmlParser.parseLatestMovie();
        }

        private void loadTopPivotItem()
        {
            // Get top 250 movie
            top250LongListSelector.ItemsSource = Top250HtmlParser.observableMovieList;
            loadMoreButton.IsEnabled = false;
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            Top250HtmlParser.indicator = indicator;
            Top250HtmlParser.loadText = loadText;
            Top250HtmlParser.loadMoreButton = loadMoreButton;
            Top250HtmlParser.parseTop250();

        }

        private void loadUSBoxPivotItem() 
        {
            // Get us box
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            USBoxJsonParser.usboxLongListSelector = usboxLongListSelector;
            USBoxJsonParser.indicator = indicator;
            USBoxJsonParser.parseUSBox();
        }

        private void loadReviewPivotItem()
        {
            // Get hot review
            loadMoreReviewButton.IsEnabled = false;
            SystemTray.Opacity = 0;
            SystemTray.IsVisible = true;
            ProgressIndicator indicator = new ProgressIndicator();
            SystemTray.ProgressIndicator = indicator;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.Text = "加载中...";
            SystemTray.ProgressIndicator.IsVisible = true;
            hotReviewLongListSelector.ItemsSource = HotReviewHtmlParser.reviewCollection;
            HotReviewHtmlParser.indicator = indicator;
            HotReviewHtmlParser.buttonText = loadReviewText;
            HotReviewHtmlParser.loadmoreButton = loadMoreReviewButton;
            HotReviewHtmlParser.parseHotReview();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((Pivot)sender).SelectedIndex;
            if (index == 1)
            {
                if (latestLoaded == false)
                {
                    latestLoaded = true;
                    loadLatestPivotItem();
                }
            }
            else if (index == 2)
            {
                if (top250Loaded == false)
                {
                    top250Loaded = true;
                    loadTopPivotItem();
                }
            }
            else if (index == 3)
            {
                if (usboxLoaded == false)
                {
                    usboxLoaded = true;
                    loadUSBoxPivotItem();
                }
            }
            else if (index == 4)
            {
                if (commentLoaded == false)
                {
                    commentLoaded = true;
                    loadReviewPivotItem();
                }
            }
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("tapped");
        }
    }
}