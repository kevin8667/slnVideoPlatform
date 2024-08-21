using Microsoft.AspNetCore.Mvc;
using prjFilmMember.Description;
using prjFilmMember.Helper;
using prjFilmMember.Model;

namespace Backstage.Controllers
{
    public class MemberController : BaseController
    {
        public IActionResult Index()
        {
            MemberHelper mHelper = new MemberHelper(ConnString);
            var memberList = mHelper.SelectMemberInfo();
            return View(memberList);
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
                if (memberInfo==null)
                {
                    return this.Json(new { IsSuccess = false, Msg = "Member 不存在!!" });

                }
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
