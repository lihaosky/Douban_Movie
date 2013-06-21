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
    public partial class ReviewPage : PhoneApplicationPage
    {
        private Review review;
        private ReviewHtmlParser reviewParser;
        private Popup searchPopup;

        public ReviewPage()
        {
            InitializeComponent();
            searchPopup = new Popup();
            review = App.reviewPassed;
            if (review != null)
            {
                Review r = Cache.getReview(review.id);
                if (r != null)
                {
                    review = r;
                }
                reviewParser = new ReviewHtmlParser(review);
                reviewParser.reviewStackPanel = reviewStackPanel;
                reviewParser.reviewProgressBar = ReviewProgressBar;
                reviewParser.commentProgressBar = ReviewCommentProgressBar;
                reviewParser.button = loadMoreCommentButton;
                reviewParser.text = loadCommentText;
                reviewParser.border = border;
                reviewParser.movieText = movieText;
                commentSelector.ItemsSource = reviewParser.commentCollection;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (reviewParser != null)
                {
                    reviewParser.cancelDownload();
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
                    if (reviewParser != null)
                    {
                        ReviewProgressBar.IsIndeterminate = true;
                        ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                        loadMoreCommentButton.IsEnabled = false;
                        reviewParser.parseReview();
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

        private void loadMoreCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (reviewParser != null && review != null)
            {
                ReviewCommentProgressBar.IsIndeterminate = true;
                ReviewCommentProgressBar.Visibility = System.Windows.Visibility.Visible;
                loadMoreCommentButton.IsEnabled = false;
                reviewParser.parseComment();
            }
        }


    }
}