﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class MemberCoupon
{
    /// <summary>
    /// 優惠券序號(代碼)
    /// </summary>
    public int SerialNo { get; set; }

    /// <summary>
    /// 會員編號
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 優惠券編號
    /// </summary>
    public int CouponId { get; set; }

    /// <summary>
    /// 狀態(是否兌換,是否失效)
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 失效時間
    /// </summary>
    public int? ExpireTime { get; set; }

    /// <summary>
    /// 使用時間
    /// </summary>
    public DateTime? UseTime { get; set; }

    /// <summary>
    /// 得到優惠券時間
    /// </summary>
    public DateTime GetTime { get; set; }

    /// <summary>
    /// 使用類型(線上電影票/線下電影票)
    /// </summary>
    public string ActionType { get; set; }

    /// <summary>
    /// 使用的交易單號
    /// </summary>
    public int? ActionRefNo { get; set; }

    /// <summary>
    /// 兌換的贈品編號
    /// </summary>
    public int? GiftId { get; set; }

    public virtual CouponInfo Coupon { get; set; }

    public virtual GiftInfo Gift { get; set; }

    public virtual MemberInfo Member { get; set; }
}