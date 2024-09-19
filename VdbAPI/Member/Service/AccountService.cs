using VdbAPI.Member.Dao;
using VdbAPI.Member.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Jose;
using VdbAPI.Member.ViewModel;

namespace VdbAPI.Member.Service
{
    public class AccountService
    {
     
        //檢查註冊資訊
        public bool RegisterCheck(string Email, string NickName, string MemberName, string Phone, string Password, DateTime? Birth, string Gender, string? PhotoPath, string? Address, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!IsValidEmail(Email))
            {
                errorMessage = "電子郵件格式不正確。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NickName))
            {
                errorMessage = "暱稱不能為空。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(MemberName))
            {
                errorMessage = "姓名不能為空。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone) || !IsValidPhone(Phone))
            {
                errorMessage = "手機號碼不正確。";
                return false;
            }

            if (!IsPasswordValid(Password))
            {
                errorMessage = "密碼格式不符合要求。";
                return false;
            }

            if (!Birth.HasValue)
            {
                errorMessage = "生日不能為空。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Gender))
            {
                errorMessage = "性別不能為空。";
                return false;
            }

            if (!string.IsNullOrEmpty(PhotoPath) && !IsValidImageFile(PhotoPath))
            {
                errorMessage = "圖片檔類型不正確。";
                return false;
            }

            return true; // 所有檢查通過
        }

        public bool ModifyCheck(mMemberInfo model, out string errMsg)
        {
            errMsg = string.Empty;

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
                errMsg = "生日區間異。";
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
        public bool LoginCheck(string email, string password, out string rtnMsg)
        {
            rtnMsg = string.Empty;
           
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
           return true ;
            // 假設這裡有一個方法來檢查密碼是否正確
            //todo 是否要加密?
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

        public static string CreateJwtToken(mMemberInfo mInfo)
        {
            string secretKey = "JarryYa";
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            // 使用 HMAC SHA-256 加密方式生成 JWT
            JwtAuthObject jwtObj = new JwtAuthObject();
            jwtObj.Email = mInfo.Email;
            jwtObj.MemberId = (int)mInfo.MemberID;
            jwtObj.ExpiredTime = DateTime.Now.AddMinutes(10).ToFileTime();

            string token = JWT.Encode(jwtObj, secretKeyBytes, JwsAlgorithm.HS256);

            return token;
        }

    }
}
