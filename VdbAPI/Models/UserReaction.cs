﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class UserReaction
{
    public int CountId { get; set; }

    public int MemberId { get; set; }

    public int ArticleId { get; set; }

    public bool ReactionType { get; set; }

    public virtual Article Article { get; set; }

    public virtual MemberInfo Member { get; set; }
}