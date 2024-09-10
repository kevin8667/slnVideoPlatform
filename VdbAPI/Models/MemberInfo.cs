﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class MemberInfo
{
    /// <summary>
    /// 會員編號
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 信箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 暱稱
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 會員姓名
    /// </summary>
    public string MemberName { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateOnly Birth { get; set; }

    /// <summary>
    /// 手機
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// 註冊時間
    /// </summary>
    public DateOnly RegisterDate { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 最後登入時間
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// 會員等級
    /// </summary>
    public string Grade { get; set; }

    /// <summary>
    /// 會員積分
    /// </summary>
    public int Point { get; set; }

    /// <summary>
    /// 更新人員或系統功能
    /// </summary>
    public string UpdateUser { get; set; }

    /// <summary>
    /// 更新時間(會員到期,降級,升級由系統功能或者後台更改)
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    public string Status { get; set; }

    public string PhotoPath { get; set; }

    public bool? Banned { get; set; }

    public string MemberIdentity { get; set; }

    public bool Fidoenabled { get; set; }

    public int? FidocredentialId { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<BlackList> BlackListBlockedMembers { get; set; } = new List<BlackList>();

    public virtual ICollection<BlackList> BlackListMembers { get; set; } = new List<BlackList>();

    public virtual ICollection<FriendChat> FriendChatReceivers { get; set; } = new List<FriendChat>();

    public virtual ICollection<FriendChat> FriendChatSenders { get; set; } = new List<FriendChat>();

    public virtual ICollection<FriendList> FriendListFriends { get; set; } = new List<FriendList>();

    public virtual ICollection<FriendList> FriendListMembers { get; set; } = new List<FriendList>();

    public virtual ICollection<Invite> InviteInvitedMembers { get; set; } = new List<Invite>();

    public virtual ICollection<Invite> InviteMembers { get; set; } = new List<Invite>();

    public virtual ICollection<MemberCoupon> MemberCoupons { get; set; } = new List<MemberCoupon>();

    public virtual ICollection<MemberNotice> MemberNotices { get; set; } = new List<MemberNotice>();

    public virtual ICollection<MemberPlayList> MemberPlayLists { get; set; } = new List<MemberPlayList>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PlayListCollaborator> PlayListCollaborators { get; set; } = new List<PlayListCollaborator>();

    public virtual ICollection<PointHistory> PointHistories { get; set; } = new List<PointHistory>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

    public virtual ICollection<ValidCode> ValidCodes { get; set; } = new List<ValidCode>();
}