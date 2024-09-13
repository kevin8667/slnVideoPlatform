using VdbAPI.Member.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdbAPI.Member.ViewModel;

namespace VdbAPI.Member.Dao
{
    internal class CouponDao
    {
        private string _connectionString { get; set; }
        internal CouponDao(string connection)
        {
            _connectionString = connection;
        }
        internal List<mCoupondata> GetCouponData(mCoupondata data)
        {
            using var connection = new SqlConnection(_connectionString);

            string sqlQuery = @"SELECT C.CouponID, C.CouponName, C.CouponDesc,c.GiftListId,
            C.Type, C.ExpireDate, MC.MemberID FROM CouponInfo C INNER JOIN MemberCoupon MC ON C.CouponID = MC.CouponID WHERE 1=1 ";
            List<SqlParameter> pars = new List<SqlParameter>();
            if (data.MemberID != null)
            {
                sqlQuery += @" and MC.MemberID = @MemberID ";
                pars.Add(new SqlParameter("MemberID", data.MemberID));
            }
            using var command = new SqlCommand(sqlQuery, connection);




            command.Parameters.AddRange(pars.ToArray());

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                return BindingCouponData(dataSet.Tables[0]);
            }


        }
        internal List<mCouponInfo> SelectCouponInfo(mCouponInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                string sqlQuery = "SELECT * FROM CouponInfo WHERE 1=1";
                List<SqlParameter> pars = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(data.CouponName))
                {
                    sqlQuery += " AND CouponName LIKE @CouponName";
                    pars.Add(new SqlParameter("CouponName", "%" + data.CouponName + "%"));
                }
                if (data.CouponID != null)
                {
                    sqlQuery += " AND CouponID = @CouponID";
                    pars.Add(new SqlParameter("CouponID", data.CouponID));
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        return BindingCouponInfo(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal List<mCoupondata> BindingCouponData(DataTable dt)
        {
            List<mCoupondata> mCoupondatas = new List<mCoupondata>();
            foreach (DataRow row in dt.Rows)
            {
                mCoupondata coupon = new mCoupondata();
                coupon.CouponID = Convert.ToInt32(row["CouponID"]);
                coupon.CouponName = row["CouponName"].ToString();
                coupon.CouponDesc = row["CouponDesc"].ToString();
                coupon.Type = row["Type"].ToString();
                coupon.ExpireDate = Convert.ToInt32(row["ExpireDate"]);
                coupon.GiftListID = Convert.ToInt32(row["GiftListId"]);
                mCoupondatas.Add(coupon);
            }
            return mCoupondatas;
        }

        internal List<mCouponInfo> BindingCouponInfo(DataTable dt)
        {
            List<mCouponInfo> CouponList = new List<mCouponInfo>();

            foreach (DataRow row in dt.Rows)
            {
                mCouponInfo coupon = new mCouponInfo();
                coupon.CouponID = Convert.ToInt32(row["CouponID"]);
                coupon.CouponName = row["CouponName"].ToString();
                coupon.CouponDesc = row["CouponDesc"].ToString();
                coupon.Type = row["Type"].ToString();
                coupon.DiscountFormula = row["DiscountFormula"].ToString();

                if (row["GiftListID"] != DBNull.Value)
                {
                    coupon.GiftListID = Convert.ToInt32(row["GiftListID"]);
                }

                coupon.ExpireDate = Convert.ToInt32(row["ExpireDate"]);
                coupon.Remark = row["Remark"].ToString();
                coupon.CreTime = Convert.ToDateTime(row["CreTime"]);

                CouponList.Add(coupon);
            }

            return CouponList;
        }

        internal void InsertCouponInfo(mCouponInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                string sqlQuery = "INSERT INTO CouponInfo (CouponID, CouponName, CouponDesc, Type, ExpireDate, GiftListID, DiscountFormula, Remark) " +
                                  "VALUES (@CouponID, @CouponName, @CouponDesc, @Type, @ExpireDate, @GiftListID, @DiscountFormula, @Remark)";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("CouponID", info.CouponID));
                pars.Add(new SqlParameter("CouponName", info.CouponName));
                pars.Add(new SqlParameter("CouponDesc", info.CouponDesc));
                pars.Add(new SqlParameter("Type", info.Type));
                pars.Add(new SqlParameter("ExpireDate", info.ExpireDate));
                pars.Add(new SqlParameter("GiftListID", info.GiftListID ?? (object)DBNull.Value));
                pars.Add(new SqlParameter("DiscountFormula", info.DiscountFormula ?? (object)DBNull.Value));
                pars.Add(new SqlParameter("Remark", info.Remark));


                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }

        internal void UpdateCouponInfo(mCouponInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();
                string sqlQuery = "UPDATE CouponInfo SET ";
                if (info.Process == mCouponInfo.CouponInfoProcess.UpdateInfo)
                {
                    sqlQuery += @"UPDATE CouponInfo
                                       SET 
                                           CouponName = @CouponName,
                                           CouponDesc = @CouponDesc,
                                           Type = @Type,
                                           DiscountFormula = @DiscountFormula,
                                           GiftListID = @GiftListID,
                                           ExpireDate = @ExpireDate,
                                           Remark = @Remark,
                                           CreTime = GETDATE()
                                       WHERE
                                           CouponID = @CouponID;";


                    pars.Add(new SqlParameter("@CouponID", info.CouponID));
                    pars.Add(new SqlParameter("@CouponName", info.CouponName));
                    pars.Add(new SqlParameter("@CouponDesc", info.CouponDesc));
                    pars.Add(new SqlParameter("@Type", info.Type));
                    pars.Add(new SqlParameter("@DiscountFormula", info.DiscountFormula ?? (object)DBNull.Value));
                    pars.Add(new SqlParameter("@GiftListID", info.GiftListID ?? (object)DBNull.Value));
                    pars.Add(new SqlParameter("@ExpireDate", info.ExpireDate));
                    pars.Add(new SqlParameter("@Remark", info.Remark));
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteCouponInfo(mCouponInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "DELETE FROM CouponInfo WHERE CouponId=@CouponId";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("CouponID", info.CouponID));

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
