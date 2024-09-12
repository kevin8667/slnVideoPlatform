using Jose;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using VdbAPI.Member.Helper;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;

namespace VdbAPI.Controllers
{
    public class LoginController : BaseController
    {
        [Route("api/[controller]/[action]")]
        [HttpPost]
        public ReturnResult<string> AccountLogin([FromBody]LoginInput input)
        {
            ReturnResult<string> rtn = new ReturnResult<string>();
            MemberHelper mHelper = new MemberHelper(ConnString);
            if (string.IsNullOrEmpty(input.email) || string.IsNullOrEmpty(input.password))
            {
                rtn.AlertMsg = "請填入Email / Password";
                return rtn;
            }

            var mInfo = mHelper.SelectMemberInfo(new Member.Model.mMemberInfo { Email = input.email, Password = input.password });
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

        // [Route("api/[controller]/[action]")]
        //[HttpPost]
        //public ReturnResult<string> Register([FromBody]RegisterInput input)
        //{
        //     ReturnResult<string> rtn = new ReturnResult<string>();
        //    MemberHelper mHelper = new MemberHelper(ConnString);
        //    if(!mHelper.RegisterCheck(input,out string errMsg))
        //    {
        //        rtn.AlertMsg = "註冊輸入檢查不通過";
        //        rtn.IsSuccess = false;
        //        return rtn;
        //    }
        //    else
        //    {
        //        rtn.IsSuccess = true;
        //        return rtn;
        //    }

        //}


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


    }
}
