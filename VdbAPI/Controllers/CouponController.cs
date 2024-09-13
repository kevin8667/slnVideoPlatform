using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdbAPI.Filters;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [JwtActionFilter]
    public class CouponController : BaseController
    {
       
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mCoupondata> GetCouponData()
        {
            //1123123213
            ReturnResult<mCoupondata> rtn = new ReturnResult<mCoupondata>();
            CouponHelper CHelper = new CouponHelper(ConnString);
            var mCoupon = CHelper.GetCouponData(new mCoupondata { MemberID = MemberId });
            rtn.Datas = mCoupon;
            rtn.IsSuccess = true;
            return rtn;
        }

        //    [Route("api/[controller]/[action]/{giftListId?}")]
        //    [HttpGet("{giftListId?}")]
        //    public ReturnResult<List<string>> GetGiftList(string giftListId = null)
        //    {
        //        ReturnResult<List<string>> rtn = new ReturnResult<List<string>>();
        //        GiftRelatedHelper GRHelper = new GiftRelatedHelper(ConnString);

        //        try
        //        {
        //            // 確保 GRHelper.GetSelectedGifts 返回 List<string>
        //            rtn.Datas = GRHelper.GetSelectedGifts(giftListId);
        //            rtn.IsSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            rtn.IsSuccess = false;
        //            rtn.AlertMsg = "發生錯誤: " + ex.Message;
        //        }

        //        return rtn;
        //    }
        //}
    }
}

