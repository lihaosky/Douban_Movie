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
    public partial class PopupSplash : UserControl
    {
        public PopupSplash()
        {
            InitializeComponent();
            this.progressBar.IsIndeterminate = true;
        }
    }
}