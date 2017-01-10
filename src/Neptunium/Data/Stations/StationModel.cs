﻿using Crystal3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neptunium.Data
{
    [DataContract]
    public class StationModel : ModelBase
    {
        [DataMember]
        public string Description { get; internal set; }
        [DataMember]
        public IEnumerable<string> Genres { get; internal set; }
        [DataMember]
        public string Logo { get; internal set; }
        [DataMember]
        public string Background { get; internal set; }
        [DataMember]
        public string Name { get; internal set; }
        [DataMember]
        public string Site { get; internal set; }
        [DataMember]
        public IEnumerable<string> StationMessages { get; internal set; }
        [DataMember]
        public IEnumerable<StationModelStream> Streams { get; internal set; }
        /// <summary>
        /// The locale that the majority of music played on this station originates from.
        /// </summary>
        [DataMember]
        public string PrimaryLocale { get; internal set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
