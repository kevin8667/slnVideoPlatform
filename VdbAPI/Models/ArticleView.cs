﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class ArticleView
{
    public string ThemeName { get; set; }

    public string MemberName { get; set; }

    public int ArticleId { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; }

    public int ThemeId { get; set; }

    public string ArticleContent { get; set; }

    public DateTime? PostDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? ReplyCount { get; set; }

    public bool? Lock { get; set; }

    public string ArticleImage { get; set; }

    public int? LikeCount { get; set; }

    public int? DislikeCount { get; set; }
}