using prjFilmMember.Dao;
using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prjFilmMember.Service
{
    public class AccountService
    {
        private string Conn { get; set; }
        public AccountService(string conn)
        {
            Conn = conn;
        }
        //檢查註冊資訊
        public void RegisterCheck(string email, string nickname, string name, string phone, string password, DateTime? birthday, string gender, string photoPath)
        {
            if (!IsValidEmail(email))
            {
                Console.WriteLine("電子郵件格式不正確。");
                return;
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                Console.WriteLine("暱稱不能為空。");
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("姓名不能為空。");
                return;
            }

            if (string.IsNullOrWhiteSpace(phone) || !IsValidPhone(phone))
            {
                Console.WriteLine("手機號碼不正確。");
                return;
            }

            if (!IsPasswordValid(password))
            {
                Console.WriteLine("密碼格式不符合要求。");
                return;
            }

            if (!birthday.HasValue)
            {
                Console.WriteLine("生日不能為空。");
                return;
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                Console.WriteLine("性別不能為空。");
                return;
            }

            if (!string.IsNullOrEmpty(photoPath) && !IsValidImageFile(photoPath))
            {
                Console.WriteLine("圖片檔類型不正確。");
                return;
            }

            Console.WriteLine("註冊資訊檢查通過！");
        }

        public bool ModifyCheck(MemberInfo model, out string errMsg)
        {
            errMsg = string.Empty;
            if (!IsValidEmail(model.Email))
            {
                errMsg = "電子信箱格式不正確。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.NickName))
            {
                errMsg = "暱稱為必填欄位。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.MemberName))
            {
                errMsg = "姓名不能為空。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.Phone) || !IsValidPhone(model.Phone))
            {
                errMsg = "手機號碼為十位數字。";
                return false;
            }


            if (model.Birth > DateTime.Now && model.Birth < DateTime.Now.AddYears(-120))
            {
                errMsg = "生日區間異常。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.Gender))
            {
                errMsg = "性別為必選欄位。";
                return false;
            }

            if (!string.IsNullOrEmpty(model.PhotoPath) && !IsValidImageFile(model.PhotoPath))
            {
                errMsg = "圖片檔類型不正確。";
                return false;
            }
            return true;
        }

        //檢查登入資訊
        public bool LoginCheck(string email, string password, out MemberInfo memberInfo, out string rtnMsg)
        {
            rtnMsg = string.Empty;
            memberInfo = new MemberInfo();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                rtnMsg = "請輸入電子信箱及密碼";
                return false;
            }
            if (!IsValidEmail(email))
            {
                rtnMsg = "電子信箱格式不正確。";
                return false;

            }

            // 假設這裡有一個方法來檢查密碼是否正確
            //todo 是否要加密?

            MemberDao dao = new MemberDao(Conn);
            var memberData = dao.SelectMemberInfo(new Model.MemberInfo { Email = email, Password = password });
            if (memberData.Any())
            {
                memberInfo = memberData.FirstOrDefault();
                rtnMsg = "登入成功！";
                return true;
            }
            else
            {
                rtnMsg = "密碼不正確。";
                return false;
            }

        }
        // 驗證電子郵件格式的方法
        private bool IsValidEmail(string email)
        {
            // 驗證電子郵件的正則表達式
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        // 驗證密碼是否符合要求的方法
        private bool IsPasswordValid(string password)
        {
            // 密碼長度必須大於6位
            if (password.Length <= 6)
            {
                return false;
            }

            // 必須包含至少一個大寫字母、一個小寫字母和一個數字
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                if (char.IsLower(c)) hasLowerCase = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            return hasUpperCase && hasLowerCase && hasDigit;
        }

        // 檢查密碼是否正確的佔位符方法


        // 驗證圖片檔類型的方法
        private bool IsValidImageFile(string imagePath)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExtension = System.IO.Path.GetExtension(imagePath).ToLower();
            return Array.Exists(validExtensions, ext => ext == fileExtension);
        }

        // 驗證手機號碼格式的方法
        private bool IsValidPhone(string phone)
        {
            string phonePattern = @"^\d{10}$"; // 假設手機號碼為10位數字
            return Regex.IsMatch(phone, phonePattern);
        }
    }
}
