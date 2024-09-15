using VdbAPI.Member.Dao;
using VdbAPI.Member.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdbAPI.Member.ViewModel;

namespace VdbAPI.Member.Helper
{
    public class MemberHelper
    {
        private string _connectionString { get; set; }
        public MemberHelper(string connection)
        {
            _connectionString = connection;
        }

        public void UpdateMemberInfo(mMemberInfo input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMemberInfo(input);

        }

        public void SetNoticeMessage(string memberid, string title, string content, string action) 
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.SetNoticeMessage(memberid, title, content, action);
        }

       
        public void DeleteNoticeMessage(int memberNoticeID)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.DeleteNoticeMessage(memberNoticeID);
        }



        public void InsertMember(RegisterViewModel input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.InsertMember(input);

        }

        public void UpdateMemberPic(mMemberInfo input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMemberPic(input);
        }
        public List<mMemberInfo> SelectMemberInfo(mMemberInfo input = null)
        {
            if (input == null)
            {
                input = new mMemberInfo();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.SelectMemberInfo(input);
        }

        public List<mMemberInfo> GetMemberData(mMemberInfo input = null)
        {
            if (input == null)
            {
                input = new mMemberInfo();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetMemberData(input);
        }

        public mMemberInfo GetEmail(string Email)
        {
            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetEmail(Email);
        }

        public List<memberFriends> GetFriendList(int memberId)
        {
            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetFriendList(memberId);
        }



        public void InviteFriend(int memberId, int friendId, string message, string status)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.InviteFriend(memberId, friendId, message, status);
        }

        public void AddFriend(int memberId, int friendId,string status)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.AddFriend(memberId, friendId, status);
        }

        public List<mMemberNotice> GetMemberNotice(mMemberNotice input = null)
        {
            if (input == null)
            {
                input = new mMemberNotice();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetMemberNotice(input);
        }

        public void DeleteFriend(int memberId, int friendId)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.DeleteFriend(memberId, friendId);
            dao.DeleteFriend(friendId, memberId);
        }

        public bool UpdatePWD(string email, string newPwd)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdatePWD(email, newPwd);
            return true;
        }

    }
}
