﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class PlayListCollaborator
{
    public int CollaboratorId { get; set; }

    public int PlayListId { get; set; }

    public int MemberId { get; set; }

    public DateTime CollaboratorJoinedAt { get; set; }

    public string CollaboratorActionType { get; set; }

    public DateTime ActionTimestamp { get; set; }

    public virtual MemberInfo Member { get; set; }

    public virtual PlayList PlayList { get; set; }
}