using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdbAPI.Filters;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;
using VdbAPI.Models;
using VdbAPI.Member.Service;

namespace VdbAPI.Controllers
{
    [JwtActionFilter]
    public class MemberController : BaseController
    {
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mMemberInfo> GetMemberData(int MemberID)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = MemberId });
            rtn.Data = mInfo.FirstOrDefault();
            rtn.IsSuccess = true;
            return rtn;
        }

        [HttpGet("memberId")]
        public IActionResult GetMemberId()
        {
            int memberId = MemberId; // 從 BaseController 獲取 MemberId
            return Ok(new { MemberId = memberId });
        }

        //[Route("api/[controller]/[action]")]
        //[HttpGet]
        //public ReturnResult<memberFriends> GetFriendList()
        //{
        //    ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
        //    MemberHelper mHelper = new MemberHelper(ConnString);
        //    var mInfo = mHelper.GetFriendList(new memberFriends { FriendID = MemberId });
        //    if (mInfo != null)
        //    {
        //        rtn.Datas = mInfo;
        //        rtn.IsSuccess = true;
        //        return rtn;
        //    }
        //    else
        //    {
        //        rtn.Data = null;
        //        rtn.IsSuccess = false;
        //        rtn.AlertMsg = "無好友";
        //        return rtn;
        //    }
        //}

        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mMemberNotice> GetMemberNotice()
        {
            ReturnResult<mMemberNotice> rtn = new ReturnResult<mMemberNotice>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.GetMemberNotice(new mMemberNotice { MemberID = MemberId, Action = "System" });
            if (mInfo != null)
            {
                rtn.Datas = mInfo;
                rtn.IsSuccess = true;
                return rtn;
            }
            else
            {
                rtn.Data = null;
                rtn.IsSuccess = false;
                rtn.AlertMsg = "無訊息";
                return rtn;
            }
        }

        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mMemberInfo> GetMemberInfo(string friendId)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.GetMemberData(new mMemberInfo { MemberID = MemberId });
            if (mInfo != null)
            {
                rtn.Datas = mInfo;
                rtn.IsSuccess = true;
                return rtn;
            }
            else
            {
                rtn.Data = null;
                rtn.IsSuccess = false;
                rtn.AlertMsg = "無好友";
                return rtn;
            }
        }

        //[Route("api/[controller]/[action]")]
        //[HttpPost]
        //public ReturnResult<memberFriends> InviteFriend(string friendId, string message)
        //{
        //    ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
        //    MemberHelper mHelper = new MemberHelper(ConnString);
        //    mHelper.InviteFriend(new memberFriends { FriendID = MemberId, Message = message });

        //    rtn.IsSuccess = true;  // 假設執行成功
        //    return rtn;
        //}



        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mMemberNotice> GetMemberNoticeAll()
        {
            ReturnResult<mMemberNotice> rtn = new ReturnResult<mMemberNotice>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.GetMemberNotice(new mMemberNotice { MemberID = MemberId });
            if (mInfo != null)
            {
                rtn.Datas = mInfo;
                rtn.IsSuccess = true;
                return rtn;
            }
            else
            {
                rtn.Data = null;
                rtn.IsSuccess = false;
                rtn.AlertMsg = "無訊息";
                return rtn;
            }
        }

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult<mMemberInfo> UpdateMemberPic(IFormFile file)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine("uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                mMemberInfo memberInfo = new mMemberInfo
                {
                    MemberID = MemberId,
                    PhotoPath = filePath
                };


                MemberHelper mHelper = new MemberHelper(ConnString);
                mHelper.UpdateMemberPic(memberInfo);
                rtn.IsSuccess = true;
            }
            else
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "無效的文件";
            }
            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpPut]
        public ReturnResult<mMemberInfo> PutMemberData(mMemberInfo memberData)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            AccountService aS = new AccountService();

            if (aS.ModifyCheck(memberData, out string errMsg))
            {
                try
                {
                    MemberHelper mHelper = new MemberHelper(ConnString);
                    mHelper.UpdateMemberInfo(memberData);

                    rtn.IsSuccess = true;
                    rtn.Data = memberData;
                }
                catch (Exception ex)
                {
                    rtn.IsSuccess = false;
                    rtn.AlertMsg = "發生錯誤: " + ex.Message;
                }
            }
            else
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = errMsg;
            }

            return rtn;
        }


        private bool VerifyUpdateSuccess(mMemberInfo input)
        {

            return true;
        }

        [HttpPost]
        public ReturnResult<string> AddFriend(string friendId)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            mHelper.AddFriend(MemberId, friendId);
            rtn.IsSuccess = true;
            rtn.AlertMsg = "好友添加成功";
            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpDelete]
        public ReturnResult<string> DeleteFriend(string friendId, string action)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            mHelper.DeleteFriend(MemberId, friendId, action);
            rtn.IsSuccess = true;
            return rtn;
        }
    }

}
