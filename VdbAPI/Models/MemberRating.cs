﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class MemberRating
{
    public int RatingId { get; set; }

    public int MemberId { get; set; }

    public int VideoId { get; set; }

    public decimal Rating { get; set; }
}