using Backstage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using prjFilmMember.Description;
using prjFilmMember.Helper;
using prjFilmMember.Model;

namespace Backstage.Controllers
{
    public class CouponManageController : BaseController
    {
        public IActionResult Index()
        {
            CouponHelper cHelper = new CouponHelper(ConnString);
            var couponList = cHelper.SelectCouponInfo();
            return View(couponList);
        }
        public ActionResult CouponDetail(string CouponId = null)
        {
            CouponDetailViewModel cInfo = new CouponDetailViewModel();
            CouponHelper couponHelper = new CouponHelper(ConnString);
            GiftHelper gHelper = new GiftHelper(ConnString);
            cInfo.GiftList=gHelper.GetGiftListInfo(new GiftListInfo { });
            if (!string.IsNullOrEmpty(CouponId))
            {
                cInfo.Detail = couponHelper.SelectCouponInfo(new prjFilmMember.Model.CouponInfo { CouponID=Convert.ToInt32(CouponId) }).FirstOrDefault();

            
            }
            else
            {
                cInfo.Detail=new CouponInfo();
                cInfo.IsNew=true;

            }

            return View(cInfo);

        }

        public ActionResult SaveCouponDetail(CouponInfo data,bool isNew)
        {
            CouponDetailViewModel vm = new CouponDetailViewModel();
            vm.Detail = data;
            vm.IsNew=isNew;
            GiftHelper gHelper = new GiftHelper(ConnString);
            vm.GiftList=gHelper.GetGiftListInfo(new GiftListInfo { });
            ViewBag.AlertMsg="測試";
            return View("CouponDetail", vm);
        }

    }
}
