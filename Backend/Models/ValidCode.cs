﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backstage.Models;

public partial class ValidCode
{
    /// <summary>
    /// 流水號
    /// </summary>
    public int ValidCodeId { get; set; }

    /// <summary>
    /// 會員編號
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    public string ValidCode1 { get; set; }

    /// <summary>
    /// 驗證類型(忘記密碼,註冊驗證)
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 到期時間
    /// </summary>
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreTime { get; set; }

    public virtual MemberInfo Member { get; set; }
}