using VdbAPI.Member.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Helper
{
    internal class GiftRelatedHelper
    {
        private string _connectionString { get; set; }
        public GiftRelatedHelper(string connection)
        {
            _connectionString = connection;
        }

        public void GetSelectedGifts(string giftID)
        {
            GiftRelatedDao dao = new GiftRelatedDao(_connectionString);
            dao.GetSelectedGifts(giftID);
        }
    }
}
