using VdbAPI.Member.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdbAPI.Member.Model;

namespace VdbAPI.Member.Helper
{
    internal class GiftRelatedHelper
    {
        private string _connectionString { get; set; }
        public GiftRelatedHelper(string connection)
        {
            _connectionString = connection;
        }

        public List<string> GetSelectedGifts(string giftlistID)
        {
            GiftRelatedDao dao = new GiftRelatedDao(_connectionString);
            // 確保這裡調用的 dao.GetSelectedGifts 返回 List<string>
            return dao.GetSelectedGifts(giftlistID); // 確保返回值正確
        }
    }
}
