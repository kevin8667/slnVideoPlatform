using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using prjFilmMember.Description;
using prjFilmMember.Helper;
using prjFilmMember.Model;
using prjFilmMember.Service;
using static prjFilmMember.Description.MemberMappingDesc;

namespace Backstage.Controllers
{
    public class LoginController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            email=@"example8@example.com";
            password="password8";
            AccountService accService = new AccountService(ConnString);
            string rtnMsg = string.Empty;
            MemberInfo memberInfo = new MemberInfo();
            var checkLogin = accService.LoginCheck(email, password, out memberInfo, out rtnMsg);
            if (checkLogin)
            {
                HttpContext.Session.SetString("LoginAccount", JsonConvert.SerializeObject(memberInfo));

                ViewBag.AlertMsg=rtnMsg;
                return RedirectToAction("Index", "Member");

            }
            else
            {
                ViewBag.AlertMsg=rtnMsg;
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult ModifyStatus(int memberId)
        {
            try
            {
                string newStatus = string.Empty;
                //有效改失效，失效改有效


                MemberHelper helper = new MemberHelper(ConnString);
                var memberInfo = helper.SelectMemberInfo(new MemberInfo { MemberID=memberId })?.FirstOrDefault();

                if (memberInfo.Status== MemberMappingDesc.Status.inactive)
                    newStatus= MemberMappingDesc.Status.active;
                else
                    newStatus= MemberMappingDesc.Status.inactive;

                helper.UpdateMemberInfo(new MemberInfo { Status=newStatus, MemberID=memberId, Process= MemberInfo.MemberInfoProcess.UpdateStatus });
                return this.Json(new { IsSuccess = true, Msg = "更新成功!", btnText = MemberMappingDesc.Status.GetButtonText(newStatus) });
            }
            catch (Exception ex)
            {
                return this.Json(new { IsSuccess = false, Msg = "更新失敗!!" });

            }

        }
    }
}
