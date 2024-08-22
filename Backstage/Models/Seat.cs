﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Backstage.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? HallsId { get; set; }

    [DisplayName("座位排數")]
    public string RowNumber { get; set; }

    [DisplayName("座位號碼")]
    public int? SeatNumber { get; set; }

    [DisplayName("座位狀態")]
    public string SeatStatus { get; set; }

    [DisplayName("影院名稱")]
    public virtual Hall Halls { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}