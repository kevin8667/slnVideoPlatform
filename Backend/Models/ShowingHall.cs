﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class ShowingHall
{
    public int ShowingHallId { get; set; }

    public int CombinationId { get; set; }

    public int HallsId { get; set; }

    public virtual NowShowingTheater Combination { get; set; }

    public virtual Hall Halls { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}