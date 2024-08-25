using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjFilmMember.Model;
using static prjFilmMember.Model.GiftInfo;

namespace prjFilmMember.Dao
{
    internal class GiftDao
    {
        private string _connectionString { get; set; }
        internal GiftDao(string connection)
        {
            _connectionString = connection;
        }
        internal List<GiftInfo> SelectGiftInfo(GiftInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"SELECT GiftID,GiftName ,GiftDesc,Qty ,Pic  FROM GiftInfo where 1=1  
                 ";
                List<SqlParameter> pars = new List<SqlParameter>();
                if (data.GiftID!=null)
                {
                    sqlQuery +=@"  and giftId=@giftId";
                    pars.Add(new SqlParameter("giftId", data.GiftID));
                }

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        return BindingGiftInfo(dataSet.Tables[0]);
                    }
                }
            }
        }

        internal List<GiftInfo> BindingGiftInfo(DataTable dt)
        {
            List<GiftInfo> GiftList = new List<GiftInfo>();
            foreach (DataRow row in dt.Rows)
            {
                //GiftID,GiftName ,GiftDesc,Qty ,Pic
                GiftInfo giftList = new GiftInfo();
                giftList.GiftID = Convert.ToInt32(row["GiftID"]);
                giftList.GiftName = row["GiftName"].ToString();
                giftList.GiftDesc = row["GiftDesc"].ToString();
                giftList.Qty = Convert.ToInt32(row["Qty"]);
                giftList.Pic =row["Pic"].ToString();


                GiftList.Add(giftList);
            }
            return GiftList;
        }
        internal void InsertGiftInfo(GiftInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = "INSERT INTO GiftInfo (GiftId, GiftName, GiftDesc, Qty ) " +
             "VALUES ((select max(giftId)+1 from giftInfo ), @GiftName, @GiftDesc, @Qty )";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("GiftName", info.GiftName));
                pars.Add(new SqlParameter("GiftDesc", info.GiftDesc));
                pars.Add(new SqlParameter("Qty", info.Qty));

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }

        internal void UpdateGiftInfo(GiftInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();
                string sqlQuery = "Update GiftInfo set";

                if (info.Process == GiftInfoProcess.UpdateInfo)
                {
                    // SQL 查詢語句giftinfo
                    sqlQuery += " giftname=@giftname, giftdesc=@giftdesc,qty=@qty,pic=@pic where giftId=@giftid ";

                    pars.Add(new SqlParameter("giftname", info.GiftName));
                    pars.Add(new SqlParameter("giftdesc", info.GiftDesc));
                    pars.Add(new SqlParameter("qty", info.Qty));
                    pars.Add(new SqlParameter("pic", info.Pic == null ? string.Empty : info.Pic));
                    pars.Add(new SqlParameter("giftid", info.GiftID));
                }
                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }

        internal List<GiftListInfo> GetGiftListInfo(GiftListInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"SELECT giftlistid, STRING_AGG(gi.GiftName, ', ') AS GiftName
                 FROM
                       GiftList gl,GiftInfo gi
                 where 
                       gl.GiftID = gi.GiftID GROUP BY
                 gl.giftlistid 
                 ";
                List<SqlParameter> pars = new List<SqlParameter>();


                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        return BindingGiftListInfo(dataSet.Tables[0]);
                    }
                }
            }
        }

        internal List<GiftListInfo> BindingGiftListInfo(DataTable dt)
        {
            List<GiftListInfo> GiftList = new List<GiftListInfo>();
            foreach (DataRow row in dt.Rows)
            {
                GiftListInfo giftList = new GiftListInfo();
                giftList.GiftListID = Convert.ToInt32(row["GiftListID"]);
                giftList.GiftName = row["GiftName"].ToString();
                GiftList.Add(giftList);
            }
            return GiftList;
        }
        internal List<GiftListInfo> SelectGiftList(string giftlistid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"SELECT giftlistid,giftId
                 FROM
                       GiftList  where giftlistid=@giftlistid  order by giftId 
                 ";
                List<SqlParameter> pars = new List<SqlParameter>();

                pars.Add(new SqlParameter("giftlistid", giftlistid));

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        return BindingGiftList(dataSet.Tables[0]);
                    }
                }
            }
        }

        internal List<GiftListInfo> BindingGiftList(DataTable dt)
        {
            List<GiftListInfo> GiftList = new List<GiftListInfo>();
            foreach (DataRow row in dt.Rows)
            {
                GiftListInfo giftList = new GiftListInfo();
                giftList.GiftListID = Convert.ToInt32(row["GiftListID"]);
                giftList.GiftID = Convert.ToInt32(row["GiftID"].ToString());
                GiftList.Add(giftList);
            }
            return GiftList;
        }





        internal void InsertGiftListInfo(GiftListInfo Info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"Insert GiftList (GiftListId,GiftId) values (@GiftListId,@GiftId)";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("GiftListId", Info.GiftListID));
                pars.Add(new SqlParameter("GiftId", Info.GiftID));

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }


        internal void DeleteGiftList(int giftListId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"Delete from  GiftList where  giftlistId=@giftlistId ";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("giftlistId", giftListId));

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開資料庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
