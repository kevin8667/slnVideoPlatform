using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using VdbAPI.Member.ViewModel;
using VdbAPI.Member.Helper;
using VdbAPI.Models;
using VdbAPI.Member.Service;
using VdbAPI.Filters;
using VdbAPI.Member.Model;

namespace VdbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [HttpPost("callback")]
        public async Task<IActionResult> HandleCallback([FromBody] AuthCallbackRequest request)
        {
            var code = request.Code;
            ReturnResult<string> rtn = new ReturnResult<string>();
            // 交換授權碼為 access token
            var accessToken = await ExchangeCodeForTokenAsync(code);

            // 使用 access token 獲取使用者資訊
            var userInfo = await GetUserInfoAsync(accessToken);
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.userId))
            {
                MemberHelper mHelper = new MemberHelper(ConnString);
                var memberInfo = mHelper.SelectMemberInfo(new Member.Model.mMemberInfo { MemberID = MemberId });

                if (memberInfo.Any())
                {

                    mHelper.UpdateMemberInfo(new Member.Model.mMemberInfo
                    {
                        MemberID = memberInfo.FirstOrDefault().MemberID,
                        BindingLine = "Y",
                        LineUserId = userInfo.userId,
                        Process = Member.Model.mMemberInfo.MemberInfoProcess.UpdateLineUser
                    });
                    rtn.IsSuccess = true;
                    rtn.Data = "Binding";
                    rtn.AlertMsg = "已成功綁定!";
                    return rtn;
                }
                else
                {
                    rtn.AlertMsg = "查無會員資料";
                    return rtn;
                }

            // Example response
            var token = await ExchangeCodeForTokenAsync(code);

            return Ok(new { Token = token });
        }



        private async Task<string> ExchangeCodeForTokenAsync(string code)
        {
            // Implement the logic to exchange the code for an access token
            // with the Line API and return the token
            // For demonstration purposes, returning a dummy token
            return "dummy-access-token";
        }
    }


    }
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
        public string ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
    public class AuthCallbackRequest
    {
        public string Code { get; set; }
    }

    public class LineUser
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }

    }
}
