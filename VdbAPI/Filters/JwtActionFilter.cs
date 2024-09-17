using Jose;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Text;

using VdbAPI.Member.ViewModel;

namespace VdbAPI.Filters {
    public class JwtActionFilter : ActionFilterAttribute {
        private readonly string _secretKey = "JarryYa"; // 秘鑰

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","");

            if(string.IsNullOrEmpty(token)) {
                context.Result = new JsonResult(new { message = "Token is missing" }) { StatusCode = 666 };
                return;
            }

            try {
                byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
                var jwtObj = JWT.Decode<JwtAuthObject>(token,secretKeyBytes);

                long currentFileTime = DateTime.Now.ToFileTime();
                if(jwtObj.ExpiredTime < currentFileTime) {
                    context.Result = new JsonResult(new { message = "Token has expired" }) { StatusCode = 666 };
                    return;
                }

                context.HttpContext.Items["MemberId"] = jwtObj.MemberId;
            }
            catch(Exception ex) {
                context.Result = new JsonResult(new { message = "Invalid token",error = ex.Message }) { StatusCode = 666 };
            }

            base.OnActionExecuting(context);
        }
    }
}