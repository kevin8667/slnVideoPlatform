using VdbAPI.Member.Dao;
using VdbAPI.Member.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Helper
{
    public class GiftHelper
    {
        private string _connectionString { get; set; }
        public GiftHelper(string connection)
        {
            _connectionString = connection;
        }

        public void UpdateGiftInfo(mGiftInfo info)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.UpdateGiftInfo(info);
        }

        public void InsertGiftInfo(mGiftInfo info)
        {
            GiftDao dao = new GiftDao(_connectionString);
            dao.InsertGiftInfo(info);
        }

        public List<mGiftInfo> SelectGiftInfo(mGiftInfo data)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.SelectGiftInfo(data);
        }



        public List<mGiftListInfo> GetGiftListInfo(mGiftListInfo input = null)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.GetGiftListInfo(input);
        }

        public List<mGiftListInfo> SelectGiftList(string giftlistId)
        {
            GiftDao dao = new GiftDao(_connectionString);
            return dao.SelectGiftList(giftlistId);
        }



        public void InsertGiftListInfo(mGiftListInfo input)
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
