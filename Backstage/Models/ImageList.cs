﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class ImageList
{
    public int ImageId { get; set; }

    public string ImagePath { get; set; }

    public virtual ICollection<ImageForVideoList> ImageForVideoLists { get; set; } = new List<ImageForVideoList>();
}