using Jose;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.Service;
using VdbAPI.Member.ViewModel;


namespace VdbAPI.Controllers
{
    public class LoginController : BaseController
    {
        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult<string> AccountLogin([FromBody] LoginInput input)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            AccountService aS = new AccountService();
            if (aS.LoginCheck(input.email, input.password, out string rtnMsg))
            {
                var mInfo = mHelper.SelectMemberInfo(new mMemberInfo { Email = input.email, Password = input.password });
                if (mInfo == null || !mInfo.Any())
                {
                    rtn.AlertMsg = "帳號或密碼錯誤!!";
                    return rtn;
                }
                else
                {
                    string jwtToken = CreateJwtToken(mInfo.FirstOrDefault());
                    rtn.IsSuccess = true;
                    rtn.Data = jwtToken;
                    return rtn;
                }
            }
            else
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = rtnMsg;
                return rtn;
            }
        }
        //create jwt token 
        private string CreateJwtToken(mMemberInfo mInfo)
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


        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult Register([FromBody] RegisterViewModel input)
        {
            ReturnResult rtn = new ReturnResult();
            AccountService aS = new AccountService();
            MemberHelper mHelper = new MemberHelper(ConnString);

            if (aS.RegisterCheck(
                input.Email,
                input.NickName,
                input.MemberName,
                input.Phone,
                input.Password,
                input?.Birth,
                input.Gender,
                input?.PhotoPath,
                input?.Address,
                out string errorMessage))
            {
                // 註冊成功的邏輯
                var mInfo = mHelper.SelectMemberInfo(new mMemberInfo { Email = input.Email });
                if (mInfo.Any())
                {
                    rtn.IsSuccess = false;
                    rtn.AlertMsg = "電子信箱重複註冊";
                }
                else
                {
                    mHelper.InsertMember(input);
                    rtn.IsSuccess = true;
                    rtn.AlertMsg = "註冊成功";
                }
            }
            else
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = errorMessage;
            }
            return rtn;
        }

        [HttpPost]
        public ReturnResult<string> Forgetpwd(string email)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();

            // 檢查電子郵件是否有效
            if (string.IsNullOrEmpty(email))
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "電子郵件地址無效。";
                return rtn;
            }

            // 使用 AccountService 來處理密碼重置邏輯
            MemberHelper mHelper = new MemberHelper(ConnString);
            var user = mHelper.GetEmail(email); // 假設有這個方法來查找用戶

            if (user == null)
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "查無此信箱";
                return rtn;
            }

            // 生成隨機密碼
            string newPwd = GenerateRandomPassword(12); // 生成12位隨機密碼

            // 更新會員密碼
            bool updateResult = mHelper.UpdatePWD(email, newPwd); // 假設 UpdatePWD 方法已經修改以接受用戶對象和新密碼

            if (updateResult)
            {
                // 發送驗證郵件，包含新密碼
                bool emailSent = SendValidateMail(email, newPwd); // 修改 SendValidateMail 方法以返回發送結果
                rtn.IsSuccess = emailSent;
                rtn.AlertMsg = emailSent ? "密碼重置郵件已發送" : "發送郵件失敗，請重試。";
            }
            else
            {
                rtn.IsSuccess = false;
                rtn.AlertMsg = "更新密碼失敗，請重試。";
            }

            return rtn;
        }

        // 發送驗證郵件方法，接收新密碼作為參數
        private bool SendValidateMail(string email, string newPwd)
        {
            string fromEmail = "jarry6304@gmail.com";
            string password = "mlqr qyss afwq vahj"; // 請考慮將密碼存儲在安全的位置

            // 設置收件人郵箱
            string toEmail = "littletree04240@gmail.com";

            // 創建郵件實例
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "重設密碼通知",
                IsBodyHtml = true,
                Body = GenerateEmailBody(newPwd) // 使用生成的 HTML 內容
            };
            mail.To.Add(new MailAddress(toEmail));

            // 設置SMTP服務器地址和端口，例如Gmail的SMTP設置
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                // 設置客戶端身份驗證 (Your credentials)
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;

                try
                {
                    // 發送郵件
                    smtpClient.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("發送郵件時發生錯誤：" + ex.Message);
                    return false;
                }
            }

        }

        // 生成郵件內容的 HTML
        private string GenerateEmailBody(string newPwd)
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>重設密碼郵件</title>
    </head>
    <body>
        <h2>重設密碼完成</h2>
        <p>親愛的用戶，您好！</p>
        <p>您的帳戶密碼已成功重設。</p>
        <p>以下是您的新密碼：<strong>{newPwd}</strong></p>
        <p>請使用此新密碼登入您的帳戶。</p>
        <p>如果您需要進一步的幫助，請隨時聯繫我們。</p>
        <p>謝謝！</p>
    </body>
    </html>";
        }

        // 生成隨機密碼方法，接收長度作為參數
        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // 假設的 UpdatePWD 方法範例
        private bool UpdatePWD(mMemberInfo info, string newPwd)
        {
            // 更新會員密碼的邏輯
            // 返回 true 表示成功，false 表示失敗
            return true; // 這裡應該替換為實際的更新邏輯
        }

    }
}
