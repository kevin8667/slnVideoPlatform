using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class catchCss : ControllerBase
    {
        
        [HttpPost]
        public ActionResult GetMatchedCss([FromBody] string cssContent)
        {
            if (string.IsNullOrEmpty(cssContent))
            {
                return BadRequest("CSS content is required.");
            }

            // 需要篩選的CSS class名稱
            List<string> classNames = new List<string>
    {
        "mem_page_border",
        "table-container",
        "general-table",
        "text_center",
        "text_left",
        "pic",
        "blue",
        "t_red",
        "t_bold",
        "info",
        "td_gray",
        "selected"
    };

            List<string> matchedStyles = new List<string>();

            // 使用正則表達式來匹配CSS class名稱
            foreach (var className in classNames)
            {
                string pattern = $@"\.{className}\s*\{{[^}}]*\}}";
                MatchCollection matches = Regex.Matches(cssContent, pattern);

                // 添加匹配的CSS樣式
                foreach (Match match in matches)
                {
                    matchedStyles.Add(match.Value);
                }
            }

            // 回傳匹配到的CSS樣式
            if (matchedStyles.Count == 0)
            {
                return NotFound("No matching CSS rules found.");
            }

            return Ok(matchedStyles);
        }

    }
}
