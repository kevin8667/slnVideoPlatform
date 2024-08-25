using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjFilmMember.Dao
{
    internal class GiftRelatedDao
    {
        private string _connectionString { get; set; }
        internal GiftRelatedDao(string connection)
        {
            _connectionString = connection;
        }
        internal List<string> GetSelectedGifts(string giftID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = @"SELECT gi.giftid from giftInfo gi ,giftlist gl where gl.giftid=gi.giftid and gl.giftlistid=@giftlistid 
";
                List<SqlParameter> pars = new List<SqlParameter>();

                pars.Add(new SqlParameter("giftlistid", giftID));


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

                        return BidingSelectedGifted(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal List<string> BidingSelectedGifted(DataTable dt)
        {
            List<string> rtn = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(row["GiftId"].ToString());
            }
            return rtn;
        }
    }
}
