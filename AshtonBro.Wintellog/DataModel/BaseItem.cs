﻿using AshtonBro.Wintellog.Common;
using System;
using System.Runtime.Serialization;

namespace AshtonBro.Wintellog.DataModel
{
    [DataContract]
    public abstract class BaseItem : BindableBase
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public Uri PageUri { get; set; }
    }
}
