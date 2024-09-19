using System.ComponentModel;

namespace VdbAPI.Member.ViewModel
{
    public class RegisterViewModel
    {
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

        [DisplayName("性別")]
        public string Gender { get; set; }

        [DisplayName("密碼")]
        public string Password { get; set; }

        [DisplayName("會員圖片")]
        public string? PhotoPath { get; set; }
        [DisplayName("地址")]
        public string? Address { get; set; }
    }
}
