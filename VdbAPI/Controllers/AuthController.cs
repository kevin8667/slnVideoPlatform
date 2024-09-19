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
        public async Task<ReturnResult<mMemberInfo>> HandleCallback([FromBody] AuthCallbackRequest request)
        {
            var code = request.Code;
            ReturnResult<mMemberInfo> rtn = new ReturnResult<mMemberInfo>();
            // 交換授權碼為 access token
            var accessToken = await ExchangeCodeForTokenAsync(code);

            // 使用 access token 獲取使用者資訊
            var userInfo = await GetUserInfoAsync(accessToken);
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.userId))
            {
                MemberHelper mHelper = new MemberHelper(ConnString);
                var memberInfo = mHelper.SelectMemberInfo(new Member.Model.mMemberInfo { LineUserId = userInfo.userId, BindingLine = "Y" });

                if (memberInfo.Any())
                {

                    string jwtToken = AccountService.CreateJwtToken(memberInfo.FirstOrDefault());
                    rtn.IsSuccess = true;
                    rtn.Data = memberInfo.FirstOrDefault();
                    rtn.Data.JwtToken = jwtToken;
                    return rtn;

                }
                else
                {
                    rtn.AlertMsg = "尚未綁定LINE";
                    return rtn;
                }

            }

            rtn.AlertMsg = "登入失敗!!";
            return rtn;

        }

        [JwtActionFilter]
        [HttpPost("BindingLine")]
        public async Task<ReturnResult<string>> HandleCallbackBinding([FromBody] AuthCallbackRequest request)
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

            }

            rtn.AlertMsg = "登入失敗!!";
            return rtn;

        }



        private async Task<string> ExchangeCodeForTokenAsync(string code)
        {
            var tokenUrl = "https://api.line.me/oauth2/v2.1/token";
            var clientId = "2006329488"; // 從 LINE 開發者控制台取得
            var clientSecret = "9d8c1d8fb3d8387734d75a2263ae03a6"; // 從 LINE 開發者控制台取得
            var redirectUri = "http://localhost:4200/#/auth/callback"; // 必須與 LINE 開發者控制台設定一致

            var parameters = new Dictionary<string, string>
    {
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", redirectUri },
        { "client_id", clientId },
        { "client_secret", clientSecret }
    };

            using var httpClient = new HttpClient();
            var requestContent = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync(tokenUrl, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to exchange authorization code for access token.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseContent);

            return tokenResponse.access_token;
        }


        private async Task<LineUser> GetUserInfoAsync(string accessToken)
        {
            var userInfoUrl = "https://api.line.me/v2/profile";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(userInfoUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve user info.");
            }

            var userInfo = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<LineUser>(userInfo);
        }


    }
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string tokenType { get; set; }
        public string scope { get; set; }
        public string exxpiresIn { get; set; }
        public string refreshToken { get; set; }
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
