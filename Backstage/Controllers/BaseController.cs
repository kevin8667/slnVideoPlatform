using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjFilmMember.Model;

public class BaseController : Controller
{
    public string ConnString = @"Data Source=.;Initial Catalog=VideoDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        try
        {
            // 確保 HttpContext 不為 null
            if (HttpContext != null)
            {
                // 從 Session 讀取 JSON 字串
                var jsonString = HttpContext.Session.GetString("LoginAccount");

                if (jsonString != null)
                {
                    // 反序列化 JSON 字串為物件
                    var accountInfo = JsonConvert.DeserializeObject<MemberInfo>(jsonString);
                    ViewBag.Account = accountInfo;
                }
            }
        }
        catch (Exception ex)
        {
            // 錯誤處理（例如記錄錯誤）
        }
    }

    public string MemberName
    {
        get
        {
            if (HttpContext.Session.GetString("LoginAccount") == null)
                return string.Empty;
            else
            {
                return JsonConvert.DeserializeObject<MemberInfo>(HttpContext.Session.GetString("LoginAccount")).MemberName;
            }
        }
    }

    public string MemberEmail
    {
        get
        {
            if (HttpContext.Session.GetString("LoginAccount") == null)
                return string.Empty;
            else
            {
                return JsonConvert.DeserializeObject<MemberInfo>(HttpContext.Session.GetString("LoginAccount")).Email;
            }
        }
    }

    public int? MemberID
    {
        get
        {
            if (HttpContext.Session.GetString("LoginAccount") == null)
                return 0;
            else
            {
                return JsonConvert.DeserializeObject<MemberInfo>(HttpContext.Session.GetString("LoginAccount")).MemberID;
            }
        }
    }

}
