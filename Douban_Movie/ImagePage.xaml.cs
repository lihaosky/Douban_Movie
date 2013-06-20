using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework.Media;
using PanoramaApp2.Resources;

namespace PanoramaApp2
{
    public partial class ImagePage : PhoneApplicationPage
    {
        private int index;
        private ObservableCollection<MovieImage> imageCollection;
        private MovieImage currentImage;
        private BitmapImage currentBitMap;

        public ImagePage()
        {
            InitializeComponent();
            imageCollection = App.imageCollectionPassed;
            index = imageCollection.IndexOf(App.imagePassed);
            currentImage = App.imagePassed;
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
                    loadImage();
                }
            }
        }

        private void image_ImageOpened(object sender, RoutedEventArgs e)
        {
            imageLoadingBar.IsIndeterminate = false;
            if (currentImage.bitMap == null)
            {
                currentImage.bitMap = currentBitMap;
            }
        }

        private void loadImage()
        {
            if (index < 0)
            {
                index = 0;
                return;
            }
            if (index == imageCollection.Count) {
                index = imageCollection.Count - 1;
                return;
            }
            imageLoadingBar.IsIndeterminate = true;
            currentImage = imageCollection[index];
            if (currentImage.bitMap != null)
            {
                image.Source = currentImage.bitMap;
            }
            else
            {
                currentBitMap = new BitmapImage(new Uri(currentImage.largeUrl));
                image.Source = currentBitMap;
            }
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                if (e.HorizontalVelocity < 0)
                {
                    index++;
                    loadImage();
                }
                if (e.HorizontalVelocity > 0)
                {
                    index--;
                    loadImage();
                }
            }
        }

        private void image_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult value = MessageBox.Show(AppResources.SaveMessage, "", MessageBoxButton.OKCancel);
            if (value == MessageBoxResult.OK)
            {
                if (currentImage.bitMap == null)
                {
                    MessageBox.Show(AppResources.SaveFail);
                }
                else
                {
                    WriteableBitmap wb = new WriteableBitmap(currentImage.bitMap);
                    using (MemoryStream ms = new MemoryStream()) {
                        wb.SaveJpeg(ms, currentImage.bitMap.PixelWidth, currentImage.bitMap.PixelHeight, 0, 100);
                        DateTime dt = DateTime.Now;
                        MediaLibrary lib = new MediaLibrary();
                        // This is important!!!
                        ms.Seek(0, SeekOrigin.Begin);
                        lib.SavePictureToCameraRoll(dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second, ms);
                        MessageBox.Show(AppResources.SaveSuccess);
                    }
                }
            }
        }
    }
}
