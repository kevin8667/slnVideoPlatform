﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class NowShowingTheater
{
    public int CombinationId { get; set; }

    public int? CinemaId { get; set; }

    public int? VideoId { get; set; }

    public virtual Cinema Cinema { get; set; }

    public virtual ICollection<ShowingHall> ShowingHalls { get; set; } = new List<ShowingHall>();

    public virtual VideoList Video { get; set; }
}