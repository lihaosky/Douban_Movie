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
    public partial class PeoplePage : PhoneApplicationPage
    {
        private People people = null;
        private bool movieLoaded = false;
        private bool imageLoaded = false;
        private PeopleHtmlParser peopleParser = null;

        public PeoplePage()
        {
            InitializeComponent();
            people = App.peoplePassed;
            if (people != null)
            {
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void loadMoreImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void imageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}