using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FourmImgController : ControllerBase {
        // GET: api/<fourmImgController>
        private readonly IWebHostEnvironment _web;
        private readonly IConfiguration _configuration;
        public FourmImgController(IWebHostEnvironment environment,IConfiguration configuration)
        {
            _web = environment;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult UploadUserPhoto(UserInfo user)
        {
            if(user.UserPhoto == null || user.UserPhoto.Length == 0)
                return BadRequest(new {
                    error = "圖片未填入"
                });

            try {
                // 將圖片保存到 Img 文件夾中
                string? rootPath = _web.ContentRootPath;
                if(string.IsNullOrEmpty(rootPath)) {
                    return NotFound(new {
                        error = "目標路徑不存在"
                    });
                }
                string path = Path.Combine(rootPath,"img");
                if(!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                Console.WriteLine("Image folder path: " + path);  // 输出物理路径
                // 生成文件名
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(user.UserPhoto.FileName);
                string fullPath = Path.Combine(path,newFileName);  // 完整路徑


                // 寫入文件
                using(var filestream = new FileStream(fullPath,FileMode.Create)) {
                    user.UserPhoto.CopyTo(filestream);
                }

                // 返回可用於前端的 URL
               
                return Ok(new {
                    filePath = $"https://localhost:7193/img/{newFileName}"
                });
            }
            catch(Exception ex) {
                return StatusCode(500,new {
                    error = "文件保存時發生錯誤: " + ex.Message
                });
            }
        }

        public class UserInfo {
            public IFormFile? UserPhoto {
                get;
                set;
            }
        }
    }
}