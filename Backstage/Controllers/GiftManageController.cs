using Backstage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjFilmMember.Helper;
using prjFilmMember.Model;
using System.Reflection.Metadata.Ecma335;
using static Backstage.ViewModels.GiftListMaintainViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backstage.Controllers
{
    public class GiftManageController : BaseController
    {
        public IActionResult Index()
        {
            GiftHelper gHelper = new GiftHelper(ConnString);

            var giftList = gHelper.GetGiftListInfo(new prjFilmMember.Model.GiftListInfo());
            return View(giftList);
        }

        public IActionResult GiftListDetail(string giftListId)
        {
            List<GiftListMaintainViewModel.GiftList> vm = new List<GiftListMaintainViewModel.GiftList>();
            GiftHelper gHelper = new GiftHelper(ConnString);
            var myGiftList = gHelper.SelectGiftList(giftListId);
            var giftList = gHelper.SelectGiftInfo(new prjFilmMember.Model.GiftInfo());
            foreach (var item in giftList)
            {
                vm.Add(new GiftListMaintainViewModel.GiftList
                {
                    GiftId=item.GiftID.ToString(),
                    GiftName=item.GiftName,
                    Qty=item.Qty.ToString(),
                    IsSelected=myGiftList.Where(x => x.GiftID==item.GiftID).Any()

                });
            }

            ViewBag.giftListId = giftListId;
            return View(vm);
        }

        public ActionResult SaveGiftList([FromBody] SaveInputData input)
        {
            try
            {
                GiftHelper gHelper = new GiftHelper(ConnString);
                gHelper.DeleteGiftList(Convert.ToInt32(input.giftListId));

                foreach (var item in input.selectedGifts)
                {
                    gHelper.InsertGiftListInfo(new prjFilmMember.Model.GiftListInfo { GiftListID=Convert.ToInt32(input.giftListId), GiftID=Convert.ToInt32(item) });
                }

                return this.Json(new { isSuccess = true, msg = "儲存成功!" });
            }
            catch (Exception ex)
            {

                return this.Json(new { isSuccess = false, msg = "儲存失敗!"+ex.Message });

            }
        }

        [HttpPost]
        public ActionResult SaveGift(GiftViewModel model)
        {
            GiftInfo gInfo = new GiftInfo();
            string uploadsFolder = @"/img/Gift/";
            string serverPath=  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Gift/");

            string errMsg = string.Empty;
            // 檢查是否有圖片上傳
            if (model.GiftPic != null && model.GiftPic.Length > 0)
            {
                var extension = Path.GetExtension(model.GiftPic.FileName).ToLower();
                if (extension == ".jpeg" || extension == ".jpg" || extension == ".png" || extension == ".gif")
                {
                    // 確保目標文件夾存在
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    var fileName = Path.GetFileName(model.GiftPic.FileName);
                    var filePath = Path.Combine(serverPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(ConvertToByteArray(model.GiftPic));
                    }

                    gInfo.Pic = $"{uploadsFolder}{fileName}"; // 設定圖片的相對路徑
                }
                else
                {
                    errMsg="請上傳圖片格式的檔案 (JPEG, PNG, GIF).";
                    return this.Json(new { isSuccess = false, msg = errMsg });
                }
            }
            gInfo.GiftID = model.GiftID;
            gInfo.GiftDesc = model.GiftDesc;
            gInfo.GiftName = model.GiftName;
            gInfo.Qty = Convert.ToInt32(model.Qty);
            GiftHelper gHelper = new GiftHelper(ConnString);
            if (model.GiftID == 0) // 判斷是新增還是編輯
            {
                gHelper.InsertGiftInfo(gInfo);
                return this.Json(new { isSuccess = true, msg = "新增成功" });
            }
            else
            {
                gInfo.Process= GiftInfo.GiftInfoProcess.UpdateInfo;
                gHelper.UpdateGiftInfo(gInfo);
                return this.Json(new { isSuccess = true, msg = "更新成功" });

            }


        }


        public byte[] ConvertToByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public ActionResult GetGiftDetails(string id)
        {
            GiftHelper gHelper = new GiftHelper(ConnString);
            var giftInfo = gHelper.SelectGiftInfo(new GiftInfo { GiftID=Convert.ToInt32(id) }).FirstOrDefault();

            return this.Json(giftInfo);
        }
    }
}
