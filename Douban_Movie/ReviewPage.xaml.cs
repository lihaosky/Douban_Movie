using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PanoramaApp2
{
    public partial class ReviewPage : PhoneApplicationPage
    {
        private Review review;
        private ReviewHtmlParser reviewParser;

        public ReviewPage()
        {
            InitializeComponent();
            review = App.reviewPassed;
            if (review != null)
            {
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                if (reviewParser != null)
                {
                    ReviewProgressBar.IsIndeterminate = true;
                    ReviewProgressBar.Visibility = System.Windows.Visibility.Visible;
                    reviewParser.parseReview();
                }
            }
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void loadMoreCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (reviewParser != null && review != null)
            {
                ReviewCommentProgressBar.IsIndeterminate = true;
                ReviewCommentProgressBar.Visibility = System.Windows.Visibility.Visible;
                reviewParser.parseComment();
            }
        }


    }
}