﻿using Crystal3.Model;
using Crystal3.Navigation;
using Neptunium.MediaSourceStream;
using Neptunium.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Neptunium.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [NavigationViewModel(typeof(ViewModel.AppShellViewModel))]
    public sealed partial class AppShellView : Page
    {
        public AppShellView()
        {
            this.InitializeComponent();

            this.Loaded += AppShellView_Loaded;

            WindowManager.GetNavigationManagerForCurrentWindow()
                .RegisterFrameAsNavigationService(inlineFrame, FrameLevel.Two);

            WindowManager.GetNavigationManagerForCurrentWindow()
                .GetNavigationServiceFromFrameLevel(FrameLevel.Two).NavigationServicePreNavigatedSignaled += AppShellView_NavigationServicePreNavigatedSignaled;

            this.SizeChanged += AppShellView_SizeChanged;

        }

        private void AppShellView_NavigationServicePreNavigatedSignaled(object sender, NavigationServicePreNavigatedSignaledEventArgs e)
        {
            RefreshNavigationSplitViewState(e.ViewModel);
        }

        private void AppShellView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine(e.NewSize);
        }

        private void InlineFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void RefreshNavigationSplitViewState(ViewModelBase viewModelToGoTo)
        {
            //couldn't think of a clever way to do this
            var viewModelType = viewModelToGoTo.GetType();
            if (viewModelType == typeof(StationsViewViewModel))
            {
                stationsNavButton.IsChecked = true;
            }
            else if (viewModelType == typeof(ViewModel.NowPlayingViewViewModel))
            {
                nowPlayingNavButton.IsChecked = true;
            }
            else
            {
                Debug.WriteLine("WARNING: Unimplemented navigation case - " + viewModelType.FullName);
            }
        }

        private async void AppShellView_Loaded(object sender, RoutedEventArgs e)
        {
            GoHome();

            //BackgroundMediaPlayer.Current.AutoPlay = true;
            //await PlaySomething();
        }

        private void GoHome()
        {
            WindowManager.GetNavigationManagerForCurrentWindow()
                            .GetNavigationServiceFromFrameLevel(Crystal3.Navigation.FrameLevel.Two)
                            .NavigateTo<StationsViewViewModel>();

            inlineFrame.BackStack.Clear();

            inlineFrame.Navigated += InlineFrame_Navigated;
        }

        private async Task PlaySomething()
        {
            var coverArt = new BitmapImage();
            coverArt.UriSource = new Uri("http://cdn.marketplaceimages.windowsphone.com/v8/images/dbf3e042-cd31-4d33-9609-f7f956512cf9?imageType=ws_icon_large");
            stationCoverArt.Source = coverArt;

            //AnimeNfo - http://itori.animenfo.com:443/
            //JPopsuki - http://213.239.204.252:8000/
            var mss = new ShoutcastMediaSourceStream(new Uri("http://itori.animenfo.com:443/"));
            mss.MetadataChanged += Mss_MetadataChanged;
            await mss.ConnectAsync();


            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
            BackgroundMediaPlayer.Current.MediaFailed += Current_MediaFailed;
            BackgroundMediaPlayer.Current.MediaOpened += Current_MediaOpened;
            BackgroundMediaPlayer.Current.BufferingStarted += Current_BufferingStarted;
            BackgroundMediaPlayer.Current.BufferingEnded += Current_BufferingEnded;
            BackgroundMediaPlayer.Current.SetMediaSource(mss.MediaStreamSource);
            //BackgroundMediaPlayer.Current.SetUriSource(new Uri("http://itori.animenfo.com:443/"));
            BackgroundMediaPlayer.Current.PlaybackMediaMarkerReached += Current_PlaybackMediaMarkerReached;

            await Task.Delay(5000);

            BackgroundMediaPlayer.Current.Play();
        }

        private async void Mss_MetadataChanged(object sender, ShoutcastMediaSourceStreamMetadataChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                nowPlayingTrackBox.Text = e.Title;
                nowPlayingArtistBox.Text = e.Artist;
            }));

        }

        private void Current_BufferingEnded(MediaPlayer sender, object args)
        {

        }

        private void Current_BufferingStarted(MediaPlayer sender, object args)
        {

        }

        private void Current_MediaOpened(MediaPlayer sender, object args)
        {

        }

        private void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            Debug.WriteLine("BackgroundMediaPlayer.CurrentState: " + Enum.GetName(typeof(MediaPlayerState), sender.CurrentState));

            switch (sender.CurrentState)
            {
                default:
                    break;
            }
        }

        private void Current_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {

        }

        private void Current_PlaybackMediaMarkerReached(MediaPlayer sender, PlaybackMediaMarkerReachedEventArgs args)
        {

        }

        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            Debug.WriteLine("State Change: " + e.OldState.Name + " -> " + e.NewState.Name);
        }
    }
}
