﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class StreamingPlatformList
{
    public int PlatformId { get; set; }

    public string PlatformName { get; set; }

    public virtual ICollection<VideoStreamingLinkList> VideoStreamingLinkLists { get; set; } = new List<VideoStreamingLinkList>();
}