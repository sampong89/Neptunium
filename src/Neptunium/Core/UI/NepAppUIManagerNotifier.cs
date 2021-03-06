﻿using Microsoft.Toolkit.Uwp.Notifications;
using Neptunium.Core.Media.Metadata;
using Neptunium.Core.Stations;
using System;
using System.Threading.Tasks;
using Windows.Phone.Devices.Notification;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Neptunium.Core.UI
{
    public class NepAppUIManagerNotifier
    {
        private ToastNotifier toastNotifier = null;
        private readonly TileUpdater tileUpdater = null;
        private readonly VibrationDevice vibrationDevice = null;
        public const string SongNotificationTag = "song-notif";

        internal NepAppUIManagerNotifier()
        {
            toastNotifier = ToastNotificationManager.CreateToastNotifier();
            tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();


            if (Crystal3.DeviceInformation.GetDevicePlatform() == Crystal3.Core.Platform.Mobile)
                vibrationDevice = VibrationDevice.GetDefault();
        }

        public void VibrateClick()
        {
            if ((bool)NepApp.Settings.GetSetting(AppSettings.UseHapticFeedbackForNavigation))
            {
                if (vibrationDevice != null)
                {
                    vibrationDevice?.Vibrate(TimeSpan.FromMilliseconds(32));
                }
            }
        }

        public void ShowGenericToastNotification(string title, string message, string tag)
        {
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = message,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },
                        }
                    }
                }
            };

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = tag;
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            toastNotifier.Show(notification);
        }

        public void ShowSongToastNotification(ExtendedSongMetadata metaData)
        {
            string toastLogo = FindToastLogo(metaData);

            ToastContent content = new ToastContent()
            {
                Launch = "now-playing",
                Audio = new ToastAudio()
                {
                    Silent = true,
                },
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = metaData.Track,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = metaData.Artist,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },

                            new AdaptiveText()
                            {
                                Text = metaData.StationPlayedOn,
                                HintStyle = AdaptiveTextStyle.Caption
                            },
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = toastLogo
                        }
                    }
                }
            };

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = SongNotificationTag;
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            toastNotifier.Show(notification);
        }

        internal void UpdateSongToastNotification(ExtendedSongMetadata metaData)
        {
            string toastLogo = FindToastLogo(metaData);

            ToastContent content = new ToastContent()
            {
                Launch = "now-playing",
                Audio = new ToastAudio()
                {
                    Silent = true,
                },
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = metaData.Track,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = metaData.Artist,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },

                            new AdaptiveText()
                            {
                                Text = metaData.StationPlayedOn,
                                HintStyle = AdaptiveTextStyle.Caption
                            },
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = toastLogo
                        }
                    }
                }
            };

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = SongNotificationTag;
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            //
            notification.SuppressPopup = true;
            //
            toastNotifier.Show(notification);
        }

        private static string FindToastLogo(ExtendedSongMetadata metaData)
        {
            //try and find a nice image to set for the toast
            string toastLogo = null;
            if (!string.IsNullOrWhiteSpace(metaData.Album?.AlbumCoverUrl))
            {
                toastLogo = metaData.Album?.AlbumCoverUrl;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(metaData.ArtistInfo?.ArtistImage))
                {
                    toastLogo = metaData.ArtistInfo?.ArtistImage;
                }
                else
                {
                    if (metaData.JPopAsiaArtistInfo != null)
                    {
                        if (metaData.JPopAsiaArtistInfo.ArtistImageUrl != null)
                            toastLogo = metaData.JPopAsiaArtistInfo.ArtistImageUrl.ToString();
                    }

                }
            }

            if (string.IsNullOrWhiteSpace(toastLogo))
            {
                toastLogo = metaData.StationLogo.ToString();
            }

            return toastLogo;
        }

        internal void ShowErrorToastNotification(StationItem currentStation, string title, string message)
        {
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = message,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },
                        }
                    }
                }
            };

            if (currentStation != null)
            {
                content.Visual.BindingGeneric.AppLogoOverride = new ToastGenericAppLogo()
                {
                    Source = currentStation.StationLogoUrl.ToString(),
                };
            }

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = "error";
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            toastNotifier.Show(notification);
        }

        public bool CheckIfStationTilePinned(StationItem stationItem)
        {
            if (Crystal3.DeviceInformation.GetDevicePlatform() == Crystal3.Core.Platform.Xbox) return false; //not supported

            return SecondaryTile.Exists(GetStationItemTileId(stationItem));
        }

        public async Task<bool> PinStationAsTileAsync(StationItem stationItem)
        {
            if (Crystal3.DeviceInformation.GetDevicePlatform() == Crystal3.Core.Platform.Xbox) return false; //not supported

            if (!CheckIfStationTilePinned(stationItem))
            {
                //create the secondary tile
                //https://github.com/Amrykid/Neptunium/blob/divergent-master-v0.5/src/Neptunium/ViewModel/StationInfoViewModel.cs#L57
                SecondaryTile secondaryTile = new SecondaryTile(GetStationItemTileId(stationItem));
                secondaryTile.DisplayName = stationItem.Name + " - Neptunium";
                secondaryTile.Arguments = "play-station?station=" + stationItem.Name.Replace(" ", "%20").Trim();
                secondaryTile.VisualElements.BackgroundColor = Colors.Purple; //await StationSupplementaryDataManager.GetStationLogoDominantColorAsync(stationItem);
                secondaryTile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png", UriKind.Absolute);
                secondaryTile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png", UriKind.Absolute);
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;

                if (await secondaryTile.RequestCreateAsync())
                {
                    //if we successfully created and pinned the secondary tile, let's update it as a live tile with the station's image.
                    var tiler = TileUpdateManager.CreateTileUpdaterForSecondaryTile(secondaryTile.TileId);

                    TileBindingContentAdaptive largeBindingContent = new TileBindingContentAdaptive()
                    {
                        PeekImage = new TilePeekImage()
                        {
                            Source = stationItem.StationLogoUrlOnline.ToString(),
                            AlternateText = stationItem.Name,
                            HintCrop = TilePeekImageCrop.None
                        },
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = stationItem.Name,
                                HintStyle = AdaptiveTextStyle.Body
                            },

                            new AdaptiveText()
                            {
                                Text = stationItem.Description,
                                HintWrap = true,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    };

                    TileBindingContentAdaptive mediumBindingContent = new TileBindingContentAdaptive()
                    {
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = stationItem.StationLogoUrlOnline.ToString(),
                        },
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = stationItem.Name,
                                HintStyle = AdaptiveTextStyle.Body
                            },

                            new AdaptiveText()
                            {
                                Text = stationItem.Description,
                                HintWrap = true,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    };

                    TileBindingContentAdaptive smallBindingContent = new TileBindingContentAdaptive()
                    {
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = stationItem.StationLogoUrlOnline.ToString(),
                        }
                    };

                    Func<TileBindingContentAdaptive, TileBinding> createBinding = (TileBindingContentAdaptive con) =>
                    {
                        return new TileBinding()
                        {
                            Branding = TileBranding.NameAndLogo,

                            DisplayName = stationItem.Name + " - Neptunium",

                            Content = con,

                            ContentId = stationItem.Name.GetHashCode().ToString()
                        };
                    };



                    TileContent content = new TileContent()
                    {
                        Visual = new TileVisual()
                        {
                            TileSmall = createBinding(smallBindingContent),
                            TileMedium = createBinding(smallBindingContent),
                            TileWide = createBinding(mediumBindingContent),
                            TileLarge = createBinding(largeBindingContent)
                        }
                    };

                    var tile = new TileNotification(content.GetXml());
                    tiler.Update(tile);


                    return true;
                }
            }

            return false;
        }

        internal string GetStationItemTileId(StationItem stationItem)
        {
            if (stationItem == null) throw new ArgumentNullException(nameof(stationItem));

            return stationItem.Name.GetHashCode().ToString();
        }

        public void UpdateLiveTile(SongMetadata nowPlaying)
        {
            if (Crystal3.DeviceInformation.GetDevicePlatform() == Crystal3.Core.Platform.Xbox) return; //not supported

            var tiler = TileUpdateManager.CreateTileUpdaterForApplication();

            TileBindingContentAdaptive largeBindingContent = new TileBindingContentAdaptive()
            {
                PeekImage = new TilePeekImage()
                {
                    Source = nowPlaying.StationLogo.ToString(),
                    AlternateText = nowPlaying.StationPlayedOn,
                    HintCrop = TilePeekImageCrop.None
                },
                Children =
                    {
                        new AdaptiveText()
                        {
                            Text = nowPlaying.Track,
                            HintStyle = AdaptiveTextStyle.Body
                        },

                        new AdaptiveText()
                        {
                            Text = nowPlaying.Artist,
                            HintWrap = true,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                        }
                    }
            };

            TileBindingContentAdaptive mediumBindingContent = new TileBindingContentAdaptive()
            {
                PeekImage = new TilePeekImage()
                {
                    Source = nowPlaying.StationLogo.ToString(),
                    AlternateText = nowPlaying.StationPlayedOn,
                    HintCrop = TilePeekImageCrop.None
                },
                Children =
                    {
                        new AdaptiveText()
                        {
                            Text = nowPlaying.Track,
                            HintStyle = AdaptiveTextStyle.Body
                        },

                        new AdaptiveText()
                        {
                            Text = nowPlaying.Artist,
                            HintWrap = true,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                        }
                    }
            };

            TileBindingContentAdaptive smallBindingContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage()
                {
                    Source = nowPlaying.StationLogo.ToString(),
                }
            };

            ShowNowPlayingTileUpdateBase(nowPlaying, tiler, largeBindingContent, mediumBindingContent, smallBindingContent);
        }

        public void UpdateExtendedMetadataLiveTile(SongMetadata nowPlaying)
        {
            if (Crystal3.DeviceInformation.GetDevicePlatform() == Crystal3.Core.Platform.Xbox) return; //not supported
            if (NepApp.SongManager.CurrentSong != nowPlaying) return;

            var tiler = TileUpdateManager.CreateTileUpdaterForApplication();

            string imgUrl = null;

            if (NepApp.SongManager.ArtworkProcessor.SongMetadata == null) return;
            if (!NepApp.SongManager.ArtworkProcessor.SongMetadata.Equals(nowPlaying)) return;

            var albumArt = NepApp.SongManager.ArtworkProcessor.GetSongArtworkUri(Neptunium.Media.Songs.NepAppSongMetadataBackground.Album);
            var artistArt = NepApp.SongManager.ArtworkProcessor.GetSongArtworkUri(Neptunium.Media.Songs.NepAppSongMetadataBackground.Artist);

            imgUrl = albumArt?.ToString();

            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                imgUrl = artistArt?.ToString() ?? nowPlaying.StationLogo.ToString();
            }

            TileBindingContentAdaptive largeBindingContent = new TileBindingContentAdaptive()
            {
                PeekImage = new TilePeekImage()
                {
                    Source = imgUrl,
                    AlternateText = nowPlaying.StationPlayedOn,
                    HintCrop = TilePeekImageCrop.None
                },
                Children =
                    {
                        new AdaptiveText()
                        {
                            Text = nowPlaying.Track,
                            HintStyle = AdaptiveTextStyle.Body
                        },

                        new AdaptiveText()
                        {
                            Text = nowPlaying.Artist,
                            HintWrap = true,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                        }
                    }
            };

            TileBindingContentAdaptive mediumBindingContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage()
                {
                    Source = imgUrl,
                },
                Children =
                    {
                        new AdaptiveText()
                        {
                            Text = nowPlaying.Track,
                            HintStyle = AdaptiveTextStyle.Body
                        },

                        new AdaptiveText()
                        {
                            Text = nowPlaying.Artist,
                            HintWrap = true,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                        }
                    }
            };

            TileBindingContentAdaptive smallBindingContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage()
                {
                    Source = imgUrl,
                }
            };

            ShowNowPlayingTileUpdateBase(nowPlaying, tiler, largeBindingContent, mediumBindingContent, smallBindingContent);
        }

        private static void ShowNowPlayingTileUpdateBase(SongMetadata nowPlaying, TileUpdater tiler, TileBindingContentAdaptive largeBindingContent, TileBindingContentAdaptive mediumBindingContent, TileBindingContentAdaptive smallBindingContent)
        {
            Func<TileBindingContentAdaptive, TileBinding> createBinding = (TileBindingContentAdaptive con) =>
            {
                return new TileBinding()
                {
                    Branding = TileBranding.NameAndLogo,

                    DisplayName = "Now Playing on Neptunium",

                    Content = con,

                    ContentId = nowPlaying.Track.GetHashCode().ToString()
                };
            };



            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = createBinding(smallBindingContent),
                    TileWide = createBinding(mediumBindingContent),
                    TileLarge = createBinding(largeBindingContent)
                }
            };

            var tile = new TileNotification(content.GetXml());
            tiler.Update(tile);
        }

        internal void ShowStationHostedProgrammingToastNotification(StationProgram program, SongMetadata metadata)
        {
            ToastContent content = new ToastContent()
            {
                Launch = "now-playing",
                Audio = new ToastAudio()
                {
                    Silent = true,
                },
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Tuning into " + metadata.Track,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = "Hosted by " + program.Host,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },

                            new AdaptiveText()
                            {
                                Text = metadata.StationPlayedOn,
                                HintStyle = AdaptiveTextStyle.Caption
                            },
                        },
                    }
                }
            };

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = SongNotificationTag;
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            toastNotifier.Show(notification);
        }
        internal void ShowStationBlockProgrammingToastNotification(StationProgram program, SongMetadata metadata)
        {
            ToastContent content = new ToastContent()
            {
                Launch = "now-playing",
                Audio = new ToastAudio()
                {
                    Silent = true,
                },
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Tuning into " + program.Name,
                                HintStyle = AdaptiveTextStyle.Title
                            },

                            new AdaptiveText()
                            {
                                Text = program.Name,
                                HintStyle = AdaptiveTextStyle.Caption
                            },
                        },
                    }
                }
            };

            var notification = new ToastNotification(content.GetXml());
            notification.Tag = SongNotificationTag;
            notification.NotificationMirroring = NotificationMirroring.Disabled;
            toastNotifier.Show(notification);
        }
    }
}