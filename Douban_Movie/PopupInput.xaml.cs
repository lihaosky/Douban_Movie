using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace PanoramaApp2
{
    public partial class PopupInput : UserControl
    {
        public PopupInput()
        {
            InitializeComponent();
        }

        private void inputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string input = inputBox.Text.Trim();
                ((Popup)this.Parent).IsOpen = false;
                if (input != string.Empty)
                {
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/SearchPage.xaml?msg=" + input, UriKind.Relative));
                }
            }
        }
    }
}
