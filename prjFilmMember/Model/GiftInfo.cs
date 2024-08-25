using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Model
{
    public class GiftInfo
    {
        [DisplayName("贈品編號")]
        public int? GiftID { get; set; }
        [DisplayName("名稱")]
        public string GiftName { get; set; }
        [DisplayName("描述")]
        public string GiftDesc { get; set; }
        [DisplayName("數量")]
        public int Qty { get; set; }
        [DisplayName("圖片")]
        public string Pic { get; set; }

        //for display
        [DisplayName("說明")]
        public string GiftDetail { get; set; }

        public GiftInfoProcess Process { get; set; }

        public GiftInfo() { Process = GiftInfoProcess.Normal; }

        public enum GiftInfoProcess
        {
            Normal,
            UpdateInfo,
        }

    }
}
