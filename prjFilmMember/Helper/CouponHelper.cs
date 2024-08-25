using prjFilmMember.Dao;
using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Helper
{

    public class CouponHelper
    {
        private string _connectionString { get; set; }
        public CouponHelper(string connection)
        {
            _connectionString = connection;
        }

        public List<CouponInfo> SelectCouponInfo(CouponInfo input = null)
        {
            if (input == null)
            {
                input = new CouponInfo();
            }
            CouponDao dao = new CouponDao(_connectionString);
            return dao.SelectCouponInfo(input);
        }
        public void InsertCouponInfo(CouponInfo input)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.InsertCouponInfo(input);
        }

        public void UpdateCouponInfo (CouponInfo data)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.UpdateCouponInfo(data);
        }
        public void DeleteCouponInfo(CouponInfo input)
        {
            CouponDao dao = new CouponDao(_connectionString);
            dao.DeleteCouponInfo(input);
        }
    }
}
