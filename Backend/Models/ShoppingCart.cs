﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int MemberId { get; set; }

    public int ViedoPlanId { get; set; }

    public virtual MemberInfo Member { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ViedoPlanList ViedoPlan { get; set; }
}