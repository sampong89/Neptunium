﻿using System;
using Windows.UI.Xaml;

namespace Neptunium.Media
{
    public class NepAppMediaSleepTimer
    {
        private DispatcherTimer sleepTimer = new DispatcherTimer();
        private NepAppMediaPlayerManager nepAppMediaPlayerManager;

        internal bool IsSleepTimerRunning { get { return sleepTimer.IsEnabled; } }
        internal DateTime? EstimateTimeToElapse { get; private set; }

        public NepAppMediaSleepTimer(NepAppMediaPlayerManager nepAppMediaPlayerManager)
        {
            this.nepAppMediaPlayerManager = nepAppMediaPlayerManager;

            sleepTimer.Tick += SleepTimer_Tick;
        }

        internal void SetSleepTimer(TimeSpan timeToWait)
        {
            if (sleepTimer.IsEnabled)
            {
                sleepTimer.Stop();
            }

            sleepTimer.Interval = timeToWait;
            EstimateTimeToElapse = DateTime.Now.Add(timeToWait);

            sleepTimer.Start();
        }

        internal void ClearSleepTimer()
        {
            if (sleepTimer.IsEnabled)
            {
                sleepTimer.Stop();
                EstimateTimeToElapse = null;
            }
        }

        private async void SleepTimer_Tick(object sender, object e)
        {
            if (nepAppMediaPlayerManager.IsPlaying)
            {
                nepAppMediaPlayerManager.Pause();

                if (!await App.GetIfPrimaryWindowVisibleAsync())
                {
                    NepApp.UI.Notifier.ShowGenericToastNotification("Sleep Timer", "Media paused.", "sleep-timer");
                }
            }

            sleepTimer.Stop();
        }
    }
}