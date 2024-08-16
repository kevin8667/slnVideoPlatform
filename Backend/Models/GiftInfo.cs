﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class GiftInfo
{
    /// <summary>
    /// 贈品編號
    /// </summary>
    public int GiftId { get; set; }

    /// <summary>
    /// 贈品名稱
    /// </summary>
    public string GiftName { get; set; }

    /// <summary>
    /// 贈品說明
    /// </summary>
    public string GiftDesc { get; set; }

    /// <summary>
    /// 贈品數量
    /// </summary>
    public int Qty { get; set; }

    /// <summary>
    /// 贈品圖片
    /// </summary>
    public string Pic { get; set; }

    public virtual ICollection<GiftList> GiftLists { get; set; } = new List<GiftList>();

    public virtual ICollection<MemberCoupon> MemberCoupons { get; set; } = new List<MemberCoupon>();
}