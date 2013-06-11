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
using System.Windows.Media;

namespace PanoramaApp2
{
    public partial class ImagePage : PhoneApplicationPage
    {
        private ObservableCollection<MovieImage> imageCollection;
        private Boolean[] loaded;
        private int imageIndex;
        private int prevPivotIndex;

        public ImagePage()
        {
            InitializeComponent();
            imageCollection = App.imageCollectionPassed;
            imageIndex = imageCollection.IndexOf(App.imagePassed);
            prevPivotIndex = -1;
            loaded = new Boolean[imageCollection.Count];
            for (int i = 0; i < imageCollection.Count; i++)
            {
                PivotItem item = new PivotItem();
                pivot.Items.Add(item);
                loaded[i] = false;
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int index = ((Pivot)sender).SelectedIndex;
            if (prevPivotIndex == -1)
            {
                loadImage(index, imageIndex);
                prevPivotIndex = index;
                return;
            }
            bool isLeft = index > prevPivotIndex ? false : true;

            if (prevPivotIndex == 0)
            {
                if (index != 1)
                {
                    isLeft = true;
                }
                else
                {
                    isLeft = false;
                }
            }
            if (prevPivotIndex == imageCollection.Count - 1)
            {
                if (index == 0)
                {
                    isLeft = false;
                }
                else
                {
                    isLeft = true;
                }
            }
            prevPivotIndex = index;
            if (isLeft)
            {
                if (imageIndex == 0)
                {
                    imageIndex = imageCollection.Count - 1;
                }
                else
                {
                    imageIndex--;
                }
            }
            else
            {
                if (imageIndex == imageCollection.Count - 1)
                {
                    imageIndex = 0;
                }
                else
                {
                    imageIndex++;
                }
            }
            if (loaded[index])
            {
                return;
            }
            loadImage(index, imageIndex);
        }

        private void loadImage(int pivotIndex, int index)
        {
            PivotItem item = (PivotItem)pivot.Items[pivotIndex];
            Grid grid = new Grid();
            ProgressBar progressbar = new ProgressBar();
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            progressbar.Foreground = brush;
            progressbar.IsIndeterminate = true;
            progressbar.Visibility = System.Windows.Visibility.Visible;
            grid.Children.Add(progressbar);
            Image image = new Image();
            grid.Children.Add(image);
            item.Content = grid;
            image.ImageOpened += delegate(object sender, RoutedEventArgs e)
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
                loaded[pivotIndex] = true;
            };
            image.ImageFailed += delegate(object sender, ExceptionRoutedEventArgs e)
            {
                progressbar.Visibility = System.Windows.Visibility.Collapsed;
            };
            image.Hold += delegate(object sender, System.Windows.Input.GestureEventArgs e)
            {
                BitmapImage bitmap = (BitmapImage)image.Source;
                MessageBoxResult value = MessageBox.Show("保存到相册?", "", MessageBoxButton.OKCancel);
                if (value == MessageBoxResult.OK)
                {
                    if (bitmap == null)
                    {
                        MessageBox.Show("保存失败...");
                    }
                    else
                    {
                        WriteableBitmap wb = new WriteableBitmap(bitmap);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            wb.SaveJpeg(ms, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
                            DateTime dt = DateTime.Now;
                            MediaLibrary lib = new MediaLibrary();
                            // This is important!!!
                            ms.Seek(0, SeekOrigin.Begin);
                            lib.SavePictureToCameraRoll(dt.Year + "-" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second, ms);
                            MessageBox.Show("保存成功!");
                        }
                    }
                }
            };
            image.Stretch = System.Windows.Media.Stretch.Uniform;
            image.Source = new BitmapImage(new Uri(imageCollection[index].largeUrl));

        }

       /* private int index;
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
            loadImage();
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
                index = imageCollection.Count - 1;
            }
            if (index >= imageCollection.Count) {
                index = 0;
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
        }*/
    }
}