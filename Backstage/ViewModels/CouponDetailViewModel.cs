using prjFilmMember.Model;

namespace Backstage.ViewModels
{
    public class CouponDetailViewModel
    {
        public bool IsNew { get; set; }
        public CouponInfo Detail { get; set; }
        public List<GiftListInfo> GiftList { get; set; }
    }
}
