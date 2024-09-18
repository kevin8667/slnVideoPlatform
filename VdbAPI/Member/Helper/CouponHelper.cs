using VdbAPI.Member.Dao;
using VdbAPI.Member.Model;
using VdbAPI.Member.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdbAPI.Member.Helper
{

    public class CouponHelper
    {
        private string _connectionString { get; set; }
        public CouponHelper(string connection)
        {
            _connectionString = connection;
        }

        public List<mCouponInfo> SelectCouponInfo(mCouponInfo input = null)
        {
            if (input == null)
            {
                input = new mCouponInfo();
            }
            CouponDao dao = new CouponDao(_connectionString);
            return dao.SelectCouponInfo(input);
        }

        public List<mCoupondata> GetCouponData(mCoupondata input = null)
        {
            if (input == null)
            {
                input = new mCoupondata();
            }
            CouponDao dao = new CouponDao(_connectionString);
            return dao.GetCouponData(input);
        }

        public void InsertCouponInfo(mCouponInfo input)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.InsertCouponInfo(input);
        }

        public void UpdateCouponInfo (mCouponInfo data)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.UpdateCouponInfo(data);
        }
        public void DeleteCouponInfo(mCouponInfo input)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.DeleteCouponInfo(input);
        }

    }
}
