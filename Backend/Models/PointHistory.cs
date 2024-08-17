﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class PointHistory
{
    /// <summary>
    /// 積分歷史編號
    /// </summary>
    public int PointHistoryId { get; set; }

    /// <summary>
    /// 會員編號
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 積分事件(發文,交易,邀請好友)
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// 事件時間
    /// </summary>
    public DateTime CreTime { get; set; }

    /// <summary>
    /// 參照事件編號(發文,交易,邀請好友)
    /// </summary>
    public int? RefNo { get; set; }

    /// <summary>
    /// 積分
    /// </summary>
    public int Point { get; set; }

    public virtual MemberInfo Member { get; set; }
}