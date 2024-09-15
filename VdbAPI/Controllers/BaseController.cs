using Microsoft.AspNetCore.Mvc;
using System;
using VdbAPI.Member.Helper;
using VdbAPI.Member.ViewModel;

namespace VdbAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        public string ConnString = @"Data Source=.;Initial Catalog=VideoDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public string MemberPhotoPath = @"C:\Users\jarry\source\repos\slnVideoPlatform\AngularFront\src\assets\img\Member";
        public string FileSavePath = @"..\assets\img\Member";
        public int MemberId
        {
            get
            {
                // 從 HttpContext.Items 中獲取 MemberId，並進行轉型
                if (HttpContext.Items["MemberId"] != null)
                {
                    return (int)HttpContext.Items["MemberId"];
                }
                return 0;  // 如果 MemberId 不存在，則返回 0 或其他合適的值
            }
        }

        public void SetNoticeMessage(string memberid, string title, string content, string action) 
        {
            MemberHelper mHelper = new MemberHelper(memberid);
            mHelper.SetNoticeMessage(memberid, title, content, action);
        }
    }
}
