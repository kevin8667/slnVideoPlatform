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

        [Route("api/[controller]/[action]/{giftListId}")]
        [HttpGet]
        public ReturnResult<mGiftInfo> GetGiftList(string giftListId)
        {
            ReturnResult<mGiftInfo> rtn = new ReturnResult<mGiftInfo>();
            GiftHelper gHelper = new GiftHelper(ConnString);

            try
            {
                // 確保 GRHelper.GetSelectedGifts 返回 List<string>
                var giftList= gHelper.SelectGiftList(giftListId);
                rtn.Datas = new List<mGiftInfo>();
                foreach (var item in giftList)
                {

                    rtn.Datas.Add(gHelper.SelectGiftInfo(new mGiftInfo { GiftID = item.GiftID }).FirstOrDefault());
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "發生錯誤: " + ex.Message;
            }

            return rtn;
        }
    }
}


