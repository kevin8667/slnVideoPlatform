using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.ViewModel
{
    public class mCoupondata
    {

        public int MemberID { get; set; }

        public int? CouponID { get; set; }

        public string CouponName { get; set; }

        public string CouponDesc { get; set; }

        public string Type { get; set; }

        public string? DiscountFormula { get; set; }

        public int? GiftListID { get; set; }

        public int? ExpireDate { get; set; }

        public string? Remark { get; set; }

        public DateTime CreTime { get; set; }

        public int? GiftID { get; set; }
       
        public string? GiftName { get; set; }
     
        public string? GiftDesc { get; set; }
      
        public int? Qty { get; set; }
      
        public string? Pic { get; set; }

        public int? ExpireTime { get; set; }

    }
}
