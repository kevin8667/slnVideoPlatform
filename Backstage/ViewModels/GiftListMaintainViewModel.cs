namespace Backstage.ViewModels
{
    public class GiftListMaintainViewModel
    {
        public class GiftList
        {
            public string GiftId { get; set; }
            public string GiftName { get; set; }
            public string Qty { get; set; }
            public bool IsSelected { get; set; }
        }
        public class SaveInputData
        {
            public List<string> selectedGifts { get; set; }
            public string giftListId { get; set; }
        }

        public class GiftViewModel
        {
            public int GiftID { get; set; }
            public string GiftName { get; set; }
            public string GiftDesc { get; set; }
            public string Qty { get; set; }
            public IFormFile GiftPic { get; set; } // 用於圖片上傳
        }
    }
}
