﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class FriendList
{
    public int FriendListId { get; set; }

    public int MemberId { get; set; }

    public int FriendId { get; set; }

    public DateTime? CreationDate { get; set; }

    public string FriendStatus { get; set; }

    public string InvitedMessage { get; set; }

    public virtual MemberInfo Friend { get; set; }

    public virtual MemberInfo Member { get; set; }
}