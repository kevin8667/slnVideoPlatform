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
        public ReturnResult<mMemberInfo> GetMemberData()
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = MemberId });
            rtn.Data = mInfo.FirstOrDefault();
            rtn.IsSuccess = true;
            return rtn;
        }

        [Route("api/[controller]/[action]/{friendID}")]
        [HttpGet]
        public ReturnResult<mMemberInfo> GetMemberDataById(string friendID)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            MemberHelper mHelper = new MemberHelper(ConnString);

            int dFriendId;

            if (!int.TryParse(friendID, out dFriendId))
            {
                rtn.AlertMsg = "會員編號不存在!";
                return rtn;
            }

            var mInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = Convert.ToInt32(friendID) });

            if (!mInfo.Any())
                rtn.AlertMsg = "會員編號不存在!";
            else
            {
                rtn.Data = mInfo.FirstOrDefault();
                rtn.IsSuccess = true;
            }
            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpGet]
        public IActionResult GetMemberId()
        {
            int memberId = MemberId; // 從 BaseController 獲取 MemberId
            return Ok(new { MemberId = memberId });
        }

        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<memberFriends> GetFriendList()
        {
            ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.GetFriendList(MemberId);
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

        [Route("api/[controller]/[action]")]
        [HttpDelete]
        public ReturnResult<string> DeleteNoticeMessage(int memberNoticeID)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            mHelper.DeleteNoticeMessage(memberNoticeID);
            rtn.IsSuccess = true;
            rtn.AlertMsg = "已刪除訊息";
            return rtn;
        }


        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<mMemberNotice> GetMemberNoticeAll()
        {
            ReturnResult<mMemberNotice> rtn = new ReturnResult<mMemberNotice>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mNotice = mHelper.GetMemberNotice(new mMemberNotice { MemberID = MemberId });
            if (mNotice != null)
            {
                rtn.Datas = mNotice;
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
                string filePath = Path.Combine(MemberPhotoPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                mMemberInfo memberInfo = new mMemberInfo
                {
                    MemberID = MemberId,
                    PhotoPath = Path.Combine(FileSavePath, fileName)
                };


                MemberHelper mHelper = new MemberHelper(ConnString);
                mHelper.UpdateMemberPic(memberInfo);
                memberInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = MemberId }).FirstOrDefault();
                rtn.IsSuccess = true;
                rtn.Data = memberInfo;
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
        public ReturnResult<mMemberInfo> PutMemberData([FromBody] mMemberInfo memberData)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            AccountService aS = new AccountService();

            if (aS.ModifyCheck(memberData, out string errMsg))
            {
                try
                {
                    MemberHelper mHelper = new MemberHelper(ConnString);
                    memberData.Process = mMemberInfo.MemberInfoProcess.UpdateInfo;
                    mHelper.UpdateMemberInfo(memberData);

                    rtn.IsSuccess = true;
                    rtn.Data = memberData;
                    rtn.AlertMsg = "已完成修改!";
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

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult<string> AddFriend(int friendId)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var friendInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = Convert.ToInt32(friendId) });
            mHelper.AddFriend(MemberId, friendId, "已接受");
            mHelper.AddFriend(friendId, MemberId, "已接受");
            mHelper.SetNoticeMessage(MemberId.ToString(), "好友接受訊息", friendInfo.FirstOrDefault().MemberName + "已經接受你的好友邀請，恭喜你又多一位朋友囉", "Friend");
            rtn.IsSuccess = true;
            rtn.AlertMsg = "已成為好友";
            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpDelete]
        public ReturnResult<string> DeleteFriend(int friendId)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            mHelper.DeleteFriend(MemberId, friendId);
            mHelper.SetNoticeMessage(friendId.ToString(), "好友刪除訊息", "你有一位好友默默地離開了你的朋友圈", "Friend");
            rtn.IsSuccess = true;
            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult InviteFriends(string friendId, string message)
        {
            ReturnResult rtn = new ReturnResult();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var friendInfo = mHelper.SelectMemberInfo(new mMemberInfo { MemberID = Convert.ToInt32(friendId) });
            var friendList = mHelper.GetFriendList(MemberId);
            if (friendList.Where(x => x.FriendId == Convert.ToInt32(friendId)).Any())
            {
                rtn.AlertMsg = "好友已存在，不可重複邀請";
                return rtn;
            }
            if (friendInfo.Any())
            {

                mHelper.InviteFriend(MemberId, Convert.ToInt32(friendId), message, "邀請中");
                mHelper.InviteFriend(Convert.ToInt32(friendId), MemberId, message, "待回覆");
                mHelper.SetNoticeMessage(friendId, "好友邀請訊息", friendInfo.FirstOrDefault().MemberName + "正對你發出好友邀請，請到會員好友回覆邀請", "Friend");
                rtn.IsSuccess = true;
                rtn.AlertMsg = $"已發送好友邀請給{friendInfo.FirstOrDefault().MemberName}";
            }
            else
            {
                rtn.AlertMsg = "找不到會員";
            }

            return rtn;
        }
    }



}
