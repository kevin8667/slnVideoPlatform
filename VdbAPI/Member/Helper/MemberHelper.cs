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

        public void UpdateMember(RegisterViewModel input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMember(input);

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

        public mMemberInfo GetMemberInfo(string friendId)
        {
            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetMemberInfo(friendId);
        }

        public void InviteFriend(int memberId, string friendId, string message)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.InviteFriend(memberId, friendId, message);
        }

        public void AddFriend(int memberId, string friendId)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.AddFriend(memberId, friendId);
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

        public void DeleteFriend(int memberId, string friendId, string action)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.DeleteFriend(memberId, friendId, action);
        }

        public bool UpdatePWD(string email, string newPwd)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdatePWD(email, newPwd);
            return true;
        }

    }
}
