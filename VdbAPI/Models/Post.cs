﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int ArticleId { get; set; }

    public int PosterId { get; set; }

    public string PostContent { get; set; }

    public DateTime? PostDate { get; set; }

    public bool? Lock { get; set; }

    public string PostImage { get; set; }

    public int? LikeCount { get; set; }

    public int? DislikeCount { get; set; }

    public virtual Article PostNavigation { get; set; }

    public virtual MemberInfo Poster { get; set; }
}