using Microsoft.AspNetCore.Mvc;
using prjFilmMember.Description;
using prjFilmMember.Helper;
using prjFilmMember.Model;
using prjFilmMember.Service;
using static prjFilmMember.Service.AccountService;
using static Backstage.ViewModels.MemberMaintainViewModel;


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

        public IActionResult MemberDetail(string MemberId)
        {
            MemberHelper memberHelper = new MemberHelper(ConnString);
            var memberInfo = memberHelper.SelectMemberInfo(new MemberInfo { MemberID = Convert.ToInt32(MemberId) }).FirstOrDefault();
            memberInfo.PhotoPath = string.IsNullOrEmpty(memberInfo.PhotoPath) ? "/img/Member/Default.jpg" : memberInfo.PhotoPath;
            ViewBag.MemberId = MemberId;
            return View(memberInfo);
        }
        public ActionResult SaveMember(SaveInputData model)
        {
            string uploadsFolder = @"/img/Member/";
            string serverPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Member/");

            string errMsg = string.Empty;
            // 檢查是否有圖片上傳
            if (model.MemberPic != null && model.MemberPic.Length > 0)
            {
                var extension = Path.GetExtension(model.MemberPic.FileName).ToLower();
                if (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".gif")
                {
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    var fileName = Path.GetFileName(model.MemberPic.FileName);
                    var filePath = Path.Combine(serverPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(ConvertToByteArray(model.MemberPic));
                    }

                    model.PhotoPath = $"{uploadsFolder}{fileName}";
                }
                else
                {
                    errMsg = "請上傳圖片格式的檔案 (JPEG, PNG, GIF).";
                    return this.Json(new { isSuccess = false, msg = errMsg });
                }
            }
            AccountService aserv = new AccountService(ConnString);
            bool check = aserv.ModifyCheck(model, out errMsg);
            if (check)
            {
                MemberHelper mHelper = new MemberHelper(ConnString);
                model.Process = MemberInfo.MemberInfoProcess.UpdateInfo;
                mHelper.UpdateMemberInfo(model);
                return this.Json(new { isSuccess = true, msg = "會員資料修改成功!" });
            }
            else
            {
                return this.Json(new { isSuccess = false, msg = errMsg });
            }

        }

        public byte[] ConvertToByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
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
                var memberInfo = helper.SelectMemberInfo(new MemberInfo { MemberID = memberId })?.FirstOrDefault();
                if (memberInfo == null)
                {
                    return this.Json(new { IsSuccess = false, Msg = "Member 不存在!!" });

                }
                if (memberInfo.Status == MemberMappingDesc.Status.inactive)
                    newStatus = MemberMappingDesc.Status.active;
                else
                    newStatus = MemberMappingDesc.Status.inactive;

                helper.UpdateMemberInfo(new MemberInfo { Status = newStatus, MemberID = memberId, Process = MemberInfo.MemberInfoProcess.UpdateStatus });
                return this.Json(new { IsSuccess = true, Msg = "更新成功!", btnText = MemberMappingDesc.Status.GetButtonText(newStatus) });
            }
            catch (Exception ex)
            {
                return this.Json(new { IsSuccess = false, Msg = "更新失敗!!" });

            }


        }
    }
}
