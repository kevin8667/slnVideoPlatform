using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Dao
{
    internal class CouponDao
    {
        private string _connectionString { get; set; }
        internal CouponDao(string connection)
        {
            _connectionString = connection;
        }

        internal List<CouponInfo> SelectCouponInfo(CouponInfo data)
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
                if (data.CouponID!=null)
                {
                    sqlQuery += " AND CouponID = @CouponID";
                    pars.Add(new SqlParameter("CouponID", data.CouponID ));
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

        internal List<CouponInfo> BindingCouponInfo(DataTable dt)
        {
            List<CouponInfo> CouponList = new List<CouponInfo>();

            foreach (DataRow row in dt.Rows)
            {
                CouponInfo coupon = new CouponInfo();
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

        internal void InsertCouponInfo(CouponInfo info)
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

        internal void UpdateCouponInfo(CouponInfo info)
        {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                List<SqlParameter> pars = new List<SqlParameter>();
                string sqlQuery = "UPDATE CouponInfo SET ";
                if(info.Process == CouponInfo.CouponInfoProcess.UpdateInfo)
                { sqlQuery += @"UPDATE CouponInfo
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

        internal void DeleteCouponInfo(CouponInfo info)
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
