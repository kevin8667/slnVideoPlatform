using prjFilmMember.Model;
using System.ComponentModel;

namespace Backstage.ViewModels
{
    public class MemberMaintainViewModel
    {
        public class SaveInputData : MemberInfo
        {
            public IFormFile MemberPic { get; set; } // 用於圖片上傳
        }  
    }
}