﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class GenreList
{
    public int GenreId { get; set; }

    public string GenreName { get; set; }

    public virtual ICollection<GenresForVideoList> GenresForVideoLists { get; set; } = new List<GenresForVideoList>();

    public virtual ICollection<VideoList> VideoLists { get; set; } = new List<VideoList>();
}