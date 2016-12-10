﻿using Neptunium.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace Neptunium.Media.Streamers
{
    public interface IMediaStreamer: IDisposable
    {
        Task ConnectAsync(StationModel station, StationModelStream stream, IEnumerable<KeyValuePair<string, object>> props = null);
        Task DisconnectAsync();
        Task ReconnectAsync();

        bool IsConnected { get; }
        StationModel CurrentStation { get; }
        StationModelStream CurrentStream { get; }

        MediaSource Source { get; }

        IObservable<BasicSongInfo> MetadataChanged { get; }
        IObservable<Exception> ErrorOccurred { get; }
    }
}