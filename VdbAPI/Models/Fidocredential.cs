﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class Fidocredential
{
    public int FidocredentialId { get; set; }

    public int MemberId { get; set; }

    public string CredentialId { get; set; }

    public byte[] PublicKey { get; set; }

    public byte[] UserHandle { get; set; }

    public long SignatureCounter { get; set; }

    public long AverageCounter { get; set; }

    public string CredType { get; set; }

    public DateTime RegDateTime { get; set; }

    public DateTime? LastUsedDateTime { get; set; }

    public virtual MemberInfo Member { get; set; }

    public virtual ICollection<MemberInfo> MemberInfos { get; set; } = new List<MemberInfo>();
}