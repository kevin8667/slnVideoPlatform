﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class PlayList
{
    public int PlayListId { get; set; }

    public string PlayListName { get; set; }

    public string PlayListDescription { get; set; }

    public int ViewCount { get; set; }

    public int LikeCount { get; set; }

    public int AddedCount { get; set; }

    public int SharedCount { get; set; }

    public string PlayListImage { get; set; }

    public byte[] ShowImage { get; set; }

    public DateTime PlayListCreatedAt { get; set; } = DateTime.Now;

    public DateTime PlayListUpdatedAt { get; set; } = DateTime.Now;

    public DateTime AnalysisTimestamp { get; set; } = DateTime.Now;

    public virtual ICollection<MemberPlayList> MemberPlayLists { get; set; } = new List<MemberPlayList>();

    public virtual ICollection<PlayListCollaborator> PlayListCollaborators { get; set; } = new List<PlayListCollaborator>();

    public virtual ICollection<PlayListItem> PlayListItems { get; set; } = new List<PlayListItem>();
}