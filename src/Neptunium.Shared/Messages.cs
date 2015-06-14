﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptunium.Shared
{
    public static class Messages
    {
        #region Messages for the background task
        public const string PlayStationMessage = "PlayStation";
        public const string PauseMessage = "Pause";
        public const string PlayMessage = "Play";

        public const string AppSuspend = "AppSuspend";
        public const string AppResume = "AppResume";
        #endregion

        #region Messages for the UI
        public const string MetadataChangedMessage = "MetadataChangedMessage";
        #endregion
    }

    [DataContractAttribute]
    public class PlayStationMessage
    {
        public PlayStationMessage(string url, uint sampleRate, string relativePath)
        {
            StreamUrl = url;
            SampleRate = sampleRate;
            RelativePath = relativePath;
        }

        [DataMember]
        public string RelativePath { get; private set; }
        [DataMember]
        public uint SampleRate { get; private set; }
        [DataMember]
        public string StreamUrl { get; private set; }
    }

    [DataContract]
    public class MetadataChangedMessage
    {
        public MetadataChangedMessage(string track, string artist)
        {
            Track = track;
            Artist = artist;
        }

        [DataMember]
        public string Track { get; private set; }

        [DataMember]
        public string Artist { get; private set; }
    }
}