using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Model
{
    public class mCouponInfo
    {
        [DisplayName("優惠券編號")]
        public int? CouponID { get; set; }
        [DisplayName("名稱")]
        public string CouponName { get; set; }
        [DisplayName("描述")]
        public string CouponDesc { get; set; }
        [DisplayName("內容")]
        public string Type { get; set; }
        [DisplayName("算式")]
        public string? DiscountFormula { get; set; }
        [DisplayName("贈品編號")]
        public int? GiftListID { get; set; }
        [DisplayName("有效日期")]
        public int? ExpireDate { get; set; }
        [DisplayName("備註")]
        public string? Remark { get; set; }
        [DisplayName("建立時間")]
        public DateTime CreTime { get; set; }

        public CouponInfoProcess Process { get; set; }
        public mCouponInfo()
        {
            Process = CouponInfoProcess.Normal;
        }
        public enum CouponInfoProcess
        {
            Normal,
            UpdateInfo,
        }
    }
}
