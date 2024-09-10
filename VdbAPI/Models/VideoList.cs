﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class VideoList
{
    public int VideoId { get; set; }

    public int TypeId { get; set; }

    public string VideoName { get; set; }

    public int? SeriesId { get; set; }

    public int MainGenreId { get; set; }

    public int? SeasonId { get; set; }

    public int? Episode { get; set; }

    public TimeOnly? RunningTime { get; set; }

    public bool IsShowing { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public decimal Rating { get; set; }

    public decimal Popularity { get; set; }

    public int? ThumbnailId { get; set; }

    public string Lang { get; set; }

    public string Summary { get; set; }

    public int? Views { get; set; }

    public string AgeRating { get; set; }

    public string TrailerUrl { get; set; }

    public virtual ICollection<CastList> CastLists { get; set; } = new List<CastList>();

    public virtual ICollection<DirectorForVideoList> DirectorForVideoLists { get; set; } = new List<DirectorForVideoList>();

    public virtual ICollection<GenresForVideoList> GenresForVideoLists { get; set; } = new List<GenresForVideoList>();

    public virtual ICollection<ImageForVideoList> ImageForVideoLists { get; set; } = new List<ImageForVideoList>();

    public virtual ICollection<KeywordForVideoList> KeywordForVideoLists { get; set; } = new List<KeywordForVideoList>();

    public virtual GenreList MainGenre { get; set; }

    public virtual ICollection<NowShowingTheater> NowShowingTheaters { get; set; } = new List<NowShowingTheater>();

    public virtual ICollection<PlayListItem> PlayListItems { get; set; } = new List<PlayListItem>();

    public virtual SeasonList Season { get; set; }

    public virtual SeriesList Series { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

    public virtual TypeList Type { get; set; }

    public virtual ICollection<VideoStreamingLinkList> VideoStreamingLinkLists { get; set; } = new List<VideoStreamingLinkList>();

    public virtual ICollection<ViedoPlanList> ViedoPlanLists { get; set; } = new List<ViedoPlanList>();
}