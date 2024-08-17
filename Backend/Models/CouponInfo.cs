﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class CouponInfo
{
    /// <summary>
    /// 優惠券編號
    /// </summary>
    public int CouponId { get; set; }

    /// <summary>
    /// 優惠券名稱
    /// </summary>
    public string CouponName { get; set; }

    /// <summary>
    /// 優惠券說明
    /// </summary>
    public string CouponDesc { get; set; }

    /// <summary>
    /// 類型(現金券折價,贈品)
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 折價公式(ex:折50寫N-50)
    /// </summary>
    public string DiscountFormula { get; set; }

    /// <summary>
    /// 增品清單編號
    /// </summary>
    public int? GiftListId { get; set; }

    /// <summary>
    /// 效期(N天)
    /// </summary>
    public int? ExpireDate { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreTime { get; set; }

    public virtual ICollection<MemberCoupon> MemberCoupons { get; set; } = new List<MemberCoupon>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}