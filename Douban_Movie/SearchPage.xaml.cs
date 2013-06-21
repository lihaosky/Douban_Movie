using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PanoramaApp2.JsonParser;
using System.Windows.Controls.Primitives;

namespace PanoramaApp2
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private SearchJsonParser parser = null;
        private Popup searchPopup;

        public SearchPage()
        {
            InitializeComponent();
            searchPopup = new Popup();
            parser = new SearchJsonParser();
            parser.resultNumber = resultNumber;
            parser.selector = hotLongListSelector;
            parser.progressBar = progressBar;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (parser != null)
                {
                    parser.cancelDownload();
                }
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
                    string msg = "";
                    if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                    {
                        searchText.Text = msg;
                        progressBar.IsIndeterminate = true;
                        progressBar.Visibility = System.Windows.Visibility.Visible;
                        parser.search(msg);
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

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PopupInput input = new PopupInput();
            input.Width = Application.Current.Host.Content.ActualWidth;
            input.Height = Application.Current.Host.Content.ActualHeight;
            searchPopup.Child = input;
            searchPopup.IsOpen = true;
            input.inputBox.Focus();
        }

        private void hotLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hotLongListSelector != null && hotLongListSelector.SelectedItem != null)
            {
                Movie m = (Movie)hotLongListSelector.SelectedItem;
                if (m != null && m.id != string.Empty)
                {
                    App.moviePassed = m;
                    NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
                }
                hotLongListSelector.SelectedItem = null;
            }
        }
    }
}