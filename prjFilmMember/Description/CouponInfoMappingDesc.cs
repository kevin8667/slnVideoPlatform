using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Description
{
    public class CouponInfoMappingDesc
    {
        public class Type
        {
            public const string SearchDiscount = "A";
            public const string SearchGift = "B";
            public const string BothDiscount_Gift = "C";
            public static string GetDesc(string Type)
            {
                switch (Type)
                {
                    case SearchDiscount:
                        return "折扣公式";
                    case SearchGift:
                        return "送贈品";
                    case BothDiscount_Gift:
                        return "折扣又送贈品";
                    default:
                        return Type;
                }
            }
        }
    }
}
