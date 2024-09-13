using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class Fidocredential
{
    public int FidocredentialId { get; set; }

    public int MemberId { get; set; }

    public string CredentialId { get; set; } = null!;

    public byte[] PublicKey { get; set; } = null!;

    public byte[] UserHandle { get; set; } = null!;

    public long SignatureCounter { get; set; }

    public long AverageCounter { get; set; }

    public string CredType { get; set; } = null!;

    public DateTime RegDateTime { get; set; }

    public DateTime? LastUsedDateTime { get; set; }

    public virtual MemberInfo Member { get; set; } = null!;

    public virtual ICollection<MemberInfo> MemberInfos { get; set; } = new List<MemberInfo>();
}
