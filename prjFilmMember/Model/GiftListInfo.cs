using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Model
{
    public class GiftListInfo
    {
        [DisplayName("名單編號")]
        public int GiftListID { get; set; }
        [DisplayName("贈品清單")]  //把清單內的禮物合併
        public string GiftName { get; set; }
        [DisplayName("贈品編號")]
        public int? GiftID { get; set; }
    }
}
