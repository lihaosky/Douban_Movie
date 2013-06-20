using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Globalization;
using System.Threading;
using PanoramaApp2.Resources;
using System.Windows.Markup;
using System.IO.IsolatedStorage;

namespace PanoramaApp2
{
    public partial class SettingPage : PhoneApplicationPage
    {
        private int currentIndex;

        public SettingPage()
        {
            currentIndex = -1;
            InitializeComponent();
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
                    String[] languages = { "简体中文", "English", "繁體中文" };
                    languagePicker.ItemsSource = languages;
                    currentIndex = 0;
                    IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
                    try
                    {
                        string languageSetting = (string)appSettings["language"];
                        if (languageSetting == "zh-CN")
                        {
                            currentIndex = 0;
                        }
                        if (languageSetting == "en-US")
                        {
                            currentIndex = 1;
                        }
                        if (languageSetting == "zh-TW")
                        {
                            currentIndex = 2;
                        }
                        languagePicker.SelectedIndex = currentIndex;
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                    languagePicker.SelectedIndex = currentIndex;
                }
            }
        }

        private void languagePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ListPicker)sender).SelectedIndex;
            if (currentIndex == -1 || index == currentIndex)
            {
                return;
            }
            currentIndex = index;
            if (index == 0)
            {
                setUI("zh-CN");
                IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
                appSettings["language"] = "zh-CN";
                appSettings.Save();

            }
            if (index == 1)
            {
                setUI("en-US");
                IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
                appSettings["language"] = "en-US";
                appSettings.Save();
            }
            if (index == 2)
            {
                setUI("zh-TW");
                IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
                appSettings["language"] = "zh-TW";
                appSettings.Save();
            }
        }


        private void setUI(string locale)
        {
            // Set this thread's current culture to the culture associated with the selected locale.
            CultureInfo newCulture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;


            // Set the FlowDirection of the RootFrame to match the new culture.
            FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
            App.RootFrame.FlowDirection = flow;

            // Set the Language of the RootFrame to match the new culture.
            App.RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

            App.mainPage.title.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.title.Text = AppResources.ApplicationTitle;

            App.mainPage.hotPivotItem.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.hotPivotItem.Header = AppResources.HotPivot;

            App.mainPage.UpcompingPivotItem.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.UpcompingPivotItem.Header = AppResources.ComingPivot;

            App.mainPage.topPivotItem.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.topPivotItem.Header = AppResources.TopPivot;

            App.mainPage.reviewPivotItem.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.reviewPivotItem.Header = AppResources.HotReviewPivot;

            App.mainPage.boxPivotItem.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.boxPivotItem.Header = AppResources.USPivot;

            App.mainPage.loadText.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.loadText.Text = AppResources.LoadMore;

            App.mainPage.loadReviewText.Language = XmlLanguage.GetLanguage(locale);
            App.mainPage.loadReviewText.Text = AppResources.LoadMore;

            App.mainPage.settingMenu.Text = AppResources.SettingMenu;
            App.mainPage.rateMenu.Text = AppResources.RateMenu;
            App.mainPage.aboutMenu.Text = AppResources.AboutMenu;
        }
    }
}