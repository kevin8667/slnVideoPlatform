﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class SeasonList
{
    public int SeasonId { get; set; }

    public int? SeriesId { get; set; }

    public string SeasonName { get; set; }

    public int? SeasonNumber { get; set; }

    public int? EpisodeCount { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string Summary { get; set; }

    public virtual SeriesList Series { get; set; }

    public virtual ICollection<VideoList> VideoLists { get; set; } = new List<VideoList>();
}