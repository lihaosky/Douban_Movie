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
using PanoramaApp2.JsonParser;
using PanoramaApp2.HtmlParser;
using PanoramaApp2.Resources;
using Microsoft.Phone.Tasks;

namespace PanoramaApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Popup popup;
        private Popup searchPopup;
        public ApplicationBarMenuItem settingMenu;
        public ApplicationBarMenuItem rateMenu;
        public ApplicationBarMenuItem aboutMenu;
        private BoolObject hotLoaded;
        private BoolObject latestLoaded;
        private BoolObject top250Loaded;
        private BoolObject usboxLoaded;
        private BoolObject commentLoaded;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            App.mainPage = this;

            hotLoaded = new BoolObject(false, false);
            latestLoaded = new BoolObject(false, false);
            top250Loaded = new BoolObject(false, false);
            usboxLoaded = new BoolObject(false, false);
            commentLoaded = new BoolObject(false, false);

            // Get hot movie
            HotMovieHtmlParser.selector = hotLongListSelector;
            popup = new Popup();
            searchPopup = new Popup();
            HotMovieHtmlParser.popup = popup;
            HotMovieHtmlParser.loaded = hotLoaded;
            
            // Create an application bar
            ApplicationBar = new ApplicationBar();
            
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            settingMenu = new ApplicationBarMenuItem(AppResources.SettingMenu);
            settingMenu.Click += settingMenu_Click;
            ApplicationBar.MenuItems.Add(settingMenu);
            
            rateMenu = new ApplicationBarMenuItem(AppResources.RateMenu);
            rateMenu.Click += rateMenu_Click;
            ApplicationBar.MenuItems.Add(rateMenu);

            aboutMenu = new ApplicationBarMenuItem(AppResources.AboutMenu);
            aboutMenu.Click += aboutMenu_Click;
            ApplicationBar.MenuItems.Add(aboutMenu);
        }

        void aboutMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        void rateMenu_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void settingMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.Relative));
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
            if (App.fromTombStone)
            {
                App.fromTombStone = false;
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                showPopup();
            }
            else
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    showPopup();
                    //HotMovieHtmlParser.parseHottMovie();
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
            TopProgressBar.IsIndeterminate = true;
            TopProgressBar.Visibility = System.Windows.Visibility.Visible;
            Top250HtmlParser.parseTop250();
        }

        private void loadReviewMoreButton_Click(object sender, RoutedEventArgs e)
        {
            loadMoreReviewButton.IsEnabled = false;
            HotReviewProgressBar.IsIndeterminate = true;
            HotReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
            HotReviewHtmlParser.progressBar = HotReviewProgressBar;
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
            if (hotReviewLongListSelector != null && hotReviewLongListSelector.SelectedItem != null)
            {
                Review review = (Review)hotReviewLongListSelector.SelectedItem;
                App.reviewPassed = review;
                NavigationService.Navigate(new Uri("/ReviewPage.xaml", UriKind.Relative));
                hotReviewLongListSelector.SelectedItem = null;
            }
        }

        private void loadLatestPivotItem()
        {
            // Get latest
            UpcomingProgressBar.IsIndeterminate = true;
            UpcomingProgressBar.Visibility = System.Windows.Visibility.Visible;
            LatestHtmlParser.selector = latestListSelector;
            LatestHtmlParser.progressbar = UpcomingProgressBar;
            LatestHtmlParser.loaded = latestLoaded;
            LatestHtmlParser.parseLatestMovie();
        }

        private void loadTopPivotItem()
        {
            // Get top 250 movie
            top250LongListSelector.ItemsSource = Top250HtmlParser.observableMovieList;
            loadMoreButton.IsEnabled = false;
            TopProgressBar.IsIndeterminate = true;
            TopProgressBar.Visibility = System.Windows.Visibility.Visible;
            Top250HtmlParser.progressBar = TopProgressBar;
            Top250HtmlParser.loadText = loadText;
            Top250HtmlParser.loadMoreButton = loadMoreButton;
            Top250HtmlParser.loaded = top250Loaded;
            Top250HtmlParser.parseTop250();

        }

        private void loadUSBoxPivotItem() 
        {
            // Get us box
            USBoxProgressBar.IsIndeterminate = true;
            USBoxProgressBar.Visibility = System.Windows.Visibility.Visible;
            USBoxJsonParser.usboxLongListSelector = usboxLongListSelector;
            USBoxJsonParser.progressBar = USBoxProgressBar;
            USBoxJsonParser.loaded = usboxLoaded;
            USBoxJsonParser.parseUSBox();
        }

        private void loadReviewPivotItem()
        {
            // Get hot review
            loadMoreReviewButton.IsEnabled = false;
            hotReviewLongListSelector.ItemsSource = HotReviewHtmlParser.reviewCollection;
            HotReviewProgressBar.IsIndeterminate = true;
            HotReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
            HotReviewHtmlParser.progressBar = HotReviewProgressBar;
            HotReviewHtmlParser.buttonText = loadReviewText;
            HotReviewHtmlParser.loadmoreButton = loadMoreReviewButton;
            HotReviewHtmlParser.loaded = commentLoaded;
            HotReviewHtmlParser.parseHotReview();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((Pivot)sender).SelectedIndex;
            if (index == 0)
            {
                if (hotLoaded.isLoaded == false && hotLoaded.isLoading == false)
                {
                    hotLoaded.isLoading = true;
                    HotMovieHtmlParser.parseHottMovie();
                }
            }

            else if (index == 1)
            {
                if (latestLoaded.isLoaded == false && latestLoaded.isLoading == false)
                {
                    latestLoaded.isLoading = true;
                    loadLatestPivotItem();
                }
            }
            else if (index == 2)
            {
                if (top250Loaded.isLoaded == false && top250Loaded.isLoading == false)
                {
                    top250Loaded.isLoading = true;
                    loadTopPivotItem();
                }
            }
            else if (index == 3)
            {
                if (usboxLoaded.isLoaded == false && usboxLoaded.isLoading == false)
                {
                    usboxLoaded.isLoading = true;
                    loadUSBoxPivotItem();
                }
            }
            else if (index == 4)
            {
                if (commentLoaded.isLoaded == false && commentLoaded.isLoading == false)
                {
                    commentLoaded.isLoading = true;
                    loadReviewPivotItem();
                }
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
    }
}