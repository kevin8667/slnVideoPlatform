using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdbAPI.Filters;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    public class MemberController : BaseController
    {
        [JwtActionFilter]
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

        [Route("api/[controller]/[action]")]
        [HttpGet]
        public ReturnResult<memberFriends> GetFriendList()
        {
            ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            var mInfo = mHelper.GetFriendList(new memberFriends { FriendID = MemberId });
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

  //      [Route("api/[controller]/[action]")]
  //      [HttpGet]
  //      public ReturnResult<mMemberInfo> GetMemberInfo(string friendId)
  //      {
  //          ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
  //          MemberHelper mHelper = new MemberHelper(ConnString);
  //          var mInfo = mHelper.GetMemberInfo(new mMemberInfo { MemberID = MemberId });
  //          if (mInfo != null)
  //          {
  //              rtn.Datas = mInfo;
  //              rtn.IsSuccess = true;
  //              return rtn;
  //          }
  //          else
  //          {
  //              rtn.Data = null;
  //              rtn.IsSuccess = false;
  //              rtn.AlertMsg = "無好友";
  //              return rtn;
  //          }
  //      }

  //    [Route("api/[controller]/[action]")]
  //      [HttpGet]
  //      public ReturnResult<memberFriends> InviteFriend(string friendId,string message)
  //      {
  //          ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
  //          MemberHelper mHelper = new MemberHelper(ConnString);
  //          var mInfo = mHelper.InviteFriend(new memberFriends { FriendID = MemberId, Message = message });
  //          if (mInfo != null)
  //          {
  //              rtn.Datas = mInfo;
  //              rtn.IsSuccess = true;
  //              return rtn;
  //          }
  //      }


  //[Route("api/[controller]/[action]")]
  //      [HttpDelete]
  //      public ReturnResult<memberFriends> DeleteFriend(string friendId,string action)
  //      {
  //          ReturnResult<memberFriends> rtn = new ReturnResult<memberFriends>();
  //          MemberHelper mHelper = new MemberHelper(ConnString);
  //          var mInfo = mHelper.DeleteFriend(new memberFriends { FriendID = MemberId, Action = action });
  //          if (mInfo != null)
  //          {
  //              rtn.Datas = mInfo;
  //              rtn.IsSuccess = true;
  //              return rtn;
  //          }
  //      }

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
         public ReturnResult<mMemberInfo> UpdateMemberPic(mMemberInfo input) 
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            if (input == null)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "請上傳照片";
                return rtn;
            }
            try
            {
                MemberHelper mHelper = new MemberHelper(ConnString);
                mHelper.UpdateMemberPic(input);


                bool isUpdated = VerifyUpdateSuccess(input);
                if (isUpdated)
                {
                    rtn.IsSuccess = true;
                    rtn.Data = input;
                }
                else
                {
                    rtn.IsSuccess = false;
                    rtn.AlertMsg = "更新圖片時發生錯誤";
                }
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "發生錯誤: " + ex.Message;
            }

            return rtn;
        }

        [Route("api/[controller]/[action]")]
        [HttpPut]
        public ReturnResult<mMemberInfo> PutMemberData(mMemberInfo input)
        {
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            if (input == null)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "提供的資料無效";
                return rtn;
            }
            try
            {
                MemberHelper mHelper = new MemberHelper(ConnString);
                mHelper.UpdateMemberInfo(input);

                
                bool isUpdated = VerifyUpdateSuccess(input); 
                if (isUpdated)
                {
                    rtn.IsSuccess = true;
                    rtn.Data = input;
                }
                else
                {
                    rtn.IsSuccess = false;
                    rtn.AlertMsg = "更新資料時發生錯誤";
                }
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "發生錯誤: " + ex.Message;
            }

            return rtn;
        }

       
        private bool VerifyUpdateSuccess(mMemberInfo input)
        {
           
            return true; 
        }
    }



}
