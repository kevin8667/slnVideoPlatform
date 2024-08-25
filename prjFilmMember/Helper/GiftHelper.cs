using prjFilmMember.Dao;
using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Helper
{
    public class GiftHelper
    {
        private string _connectionString { get; set; }
        public GiftHelper(string connection)
        {
            _connectionString = connection;
        }

        public void UpdateGiftInfo(GiftInfo info)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.UpdateGiftInfo(info);
        }

        public void InsertGiftInfo(GiftInfo info)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.InsertGiftInfo(info);
        }

        public List<GiftInfo> SelectGiftInfo(GiftInfo data)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.SelectGiftInfo(data);
        }



        public List<GiftListInfo> GetGiftListInfo(GiftListInfo input = null)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.GetGiftListInfo(input);
        }

        public List<GiftListInfo> SelectGiftList(string giftlistId)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.SelectGiftList(giftlistId);
        }



        public void InsertGiftListInfo(GiftListInfo input)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.InsertGiftListInfo(input);
        }

        public void DeleteGiftList(int giftListId)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.DeleteGiftList(giftListId);
        }
    }
}
