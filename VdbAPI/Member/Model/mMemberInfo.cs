using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Model
{
    public class mMemberInfo
    {
        [DisplayName("會員編碼")]
        public int? MemberID { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("暱稱")]
        public string NickName { get; set; }

        [DisplayName("姓名")]
        public string MemberName { get; set; }

        [DisplayName("生日")]
        public DateTime Birth { get; set; }

        [DisplayName("電話")]
        public string Phone { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("性別")]
        public string Gender { get; set; }

        [DisplayName("註冊日期")]
        public DateTime RegisterDate { get; set; }

        [DisplayName("密碼")]
        public string Password { get; set; }

        [DisplayName("最後登入日期")]
        public DateTime LastLoginDate { get; set; }

        [DisplayName("等級")]
        public string Grade { get; set; }

        [DisplayName("積分")]
        public int Point { get; set; }

        [DisplayName("更新者")]
        public string UpdateUser { get; set; }

        [DisplayName("更新時間")]
        public DateTime UpdateTime { get; set; }

        [DisplayName("狀態")]
        public string Status { get; set; }

        [DisplayName("會員圖片")]
        public string? PhotoPath { get; set; }

        public bool? Banned { get; set; }

        public string? MemberIdentity { get; set; }

        public string? FIDOEnabled { get; set; }

        public int? FIDOCredentialID { get; set; }
        public string BindingLine { get; set; }
        public string LineUserId { get; set; }
        public string JwtToken { get; set; }


        public MemberInfoProcess Process { get; set; }
        public mMemberInfo()
        {
            Process = MemberInfoProcess.Normal;
        }
        public enum MemberInfoProcess
        {
            Normal,
            UpdateInfo,
            UpdatePwd,
            UpdateStatus,
            UpdateLineUser

        }
    }
}

