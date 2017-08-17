﻿using Neptunium.Glue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Neptunium.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Crystal3.Navigation.NavigationViewModel(typeof(Neptunium.ViewModel.NowPlayingPageViewModel), Crystal3.Navigation.NavigationViewSupportedPlatform.Xbox)]
    [Neptunium.Core.UI.NepAppUINoChromePage()]
    public sealed partial class XboxNowPlayingPage : Page, IXboxInputPage
    {
        public XboxNowPlayingPage()
        {
            this.InitializeComponent();
        }

        public void PreserveFocus()
        {
            
        }

        public void RestoreFocus()
        {

        }

        public void SetLeftFocus(UIElement elementToTheLeft)
        {

        }

        public void SetRightFocus(UIElement elementToTheRight)
        {

        }

        public void SetTopFocus(UIElement elementAbove)
        {

        }

        public void SetBottomFocus(UIElement elementBelow)
        {

        }
    }
}