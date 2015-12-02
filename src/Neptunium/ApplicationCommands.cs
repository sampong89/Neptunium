﻿using Crystal3.UI.Commands;
using Neptunium.Data;
using Neptunium.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptunium
{
    public class ApplicationCommands
    {
        public ApplicationCommands()
        {
            PlayStationCommand = new CRelayCommand(async station =>
                {
                    if (Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile() != null)
                    {
                        var result = await ShoutcastStationMediaPlayer.PlayStationAsync((StationModel)station);
                    }
                    else
                    {
                        var dialogService = Crystal3.IOC.IoCManager.Resolve<Crystal3.Core.IMessageDialogService>();

                        if (dialogService != null)
                        {
                            await dialogService.ShowAsync(
                                string.Format("We are unable to connect to {0}. You do not have a suitable internet connection.", ((StationModel)station).Name), "No Internet Connection");
                        }
                    }

                }, station => station != null);
        }

        public CRelayCommand PlayStationCommand { get; private set; }
    }
}