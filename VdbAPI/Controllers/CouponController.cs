using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdbAPI.Filters;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    public class CouponController :BaseController
    {
        [JwtActionFilter]
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mCoupondata> GetCouponData()  
        {
            ReturnResult<mCoupondata> rtn = new ReturnResult<mCoupondata>();
            CouponHelper CHelper = new CouponHelper(ConnString);
            var mCoupon = CHelper.GetCouponData(new mCoupondata { MemberID = MemberId });
            rtn.Datas = mCoupon;
            rtn.IsSuccess = true;
            return rtn;
        }

        //[Route("api/[controller]/[action]/{giftListId?}")]
        //[HttpGet]
        //public ReturnResult<mGiftListInfo> GetGiftList(string giftListId = null)
        //{
        //    ReturnResult<mGiftListInfo> rtn = new ReturnResult<mGiftListInfo>();
        //    CouponHelper CHelper = new CouponHelper(ConnString);
        //    var mGift = CHelper.GetGiftList();
        //    rtn.Datas = mGift;
        //    rtn.IsSuccess = true;
        //    return rtn;
        //}

    }
}
