﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int? ViedoId { get; set; }

    public int? HallsId { get; set; }

    public DateTime? ShowTimeDatetime { get; set; }

    public virtual Hall Halls { get; set; }

    public virtual ShowingHall HallsNavigation { get; set; }

    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual VideoList Viedo { get; set; }
}