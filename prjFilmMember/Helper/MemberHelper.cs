using prjFilmMember.Dao;
using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Helper
{
    public class MemberHelper
    {
        private string _connectionString { get; set; }
        public MemberHelper(string connection)
        {
            _connectionString = connection;
        }
        public List<MemberInfo> SelectMemberInfo(MemberInfo input = null)
        {
            if (input == null)
            {
                input = new MemberInfo();
            }

            MemberDao dao = new MemberDao(_connectionString);
            return dao.SelectMemberInfo(input);
        }

        public void UpdateMemberInfo(MemberInfo input)
        {
            MemberDao dao = new MemberDao(_connectionString);
            dao.UpdateMemberInfo(input);

        }



    }
}
