﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VdbAPI.Models;

public partial class ImageForVideoList
{
    public int Id { get; set; }

    public int? ImageId { get; set; }

    public int? VideoId { get; set; }

    public virtual ImageList Image { get; set; }
    [JsonIgnore]
    public virtual VideoList Video { get; set; }
}