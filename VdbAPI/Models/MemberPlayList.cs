﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class MemberPlayList
{
    public int MemberPlayListId { get; set; }

    public int MemberId { get; set; }

    public int PlayListId { get; set; }

    public DateTime? AddedOtherMemberPlayListAt { get; set; }
}