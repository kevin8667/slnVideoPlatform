﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? HallsId { get; set; }

    public string RowNumber { get; set; }

    public int? SeatNumber { get; set; }

    public string SeatStatus { get; set; }

    public virtual Hall Halls { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}