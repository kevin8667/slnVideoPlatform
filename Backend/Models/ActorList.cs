﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class ActorList
{
    public int ActorId { get; set; }

    public string ActorName { get; set; }

    public byte[] ActorImage { get; set; }

    public virtual ICollection<CastList> CastLists { get; set; } = new List<CastList>();
}