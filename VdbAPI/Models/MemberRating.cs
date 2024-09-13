using System;
using System.Collections.Generic;

namespace VdbAPI.Models;

public partial class MemberRating
{
    public int RatingId { get; set; }

    public int MemberId { get; set; }

    public int VideoId { get; set; }

    public decimal Rating { get; set; }

    public virtual MemberInfo Member { get; set; } = null!;

    public virtual VideoList Video { get; set; } = null!;
}
