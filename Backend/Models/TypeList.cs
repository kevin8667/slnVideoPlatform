﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class TypeList
{
    public int TypeId { get; set; }

    public string TypeName { get; set; }

    public virtual ICollection<VideoList> VideoLists { get; set; } = new List<VideoList>();
}