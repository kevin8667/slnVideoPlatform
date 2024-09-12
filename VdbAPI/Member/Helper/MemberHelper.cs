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
        public List<mMemberInfo> SelectMemberInfo(mMemberInfo input = null)
        {
            if (input == null)
            {
                input = new mMemberInfo();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.SelectMemberInfo(input);
        }

        public void UpdateMemberInfo(mMemberInfo input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMemberInfo(input);

        }

        public void UpdateMemberPic(mMemberInfo input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMemberPic(input);
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

        public List<memberFriends> GetFriendList(memberFriends input = null)
        {
            if (input == null)
            {
                input = new memberFriends();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.GetFriendList(input);
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

       
    }
}
