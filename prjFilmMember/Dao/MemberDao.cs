using prjFilmMember.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static prjFilmMember.Model.MemberInfo;

namespace prjFilmMember.Dao
{
    internal class MemberDao
    {
        private string _connectionString { get; set; }
        internal MemberDao(string connection)
        {
            _connectionString = connection;
        }
        internal List<MemberInfo> SelectMemberInfo(MemberInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // SQL 查詢語句
                string sqlQuery = "SELECT * FROM MemberInfo where 1=1 ";
                List<SqlParameter> pars = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(data.MemberName))
                {
                    sqlQuery += @" and name like @name ";
                    pars.Add(new SqlParameter("name", "%" + data.MemberName + "%"));
                }
                if (!string.IsNullOrEmpty(data.Phone))
                {
                    sqlQuery += @" and Phone = @Phone ";
                    pars.Add(new SqlParameter("Phone", data.Phone));
                }
                if (!string.IsNullOrEmpty(data.Status))
                {
                    sqlQuery += @" and Status = @Status ";
                    pars.Add(new SqlParameter("Status", data.Status));
                }

                if (!string.IsNullOrEmpty(data.Email))
                {
                    sqlQuery += @" and Email = @Email ";
                    pars.Add(new SqlParameter("Email", data.Email));
                }
                if (!string.IsNullOrEmpty(data.Password))
                {
                    sqlQuery += @" and Password = @Password ";
                    pars.Add(new SqlParameter("Password", data.Password));
                }
                if (data.MemberID != null)
                {
                    sqlQuery += @" and MemberID = @MemberID ";
                    pars.Add(new SqlParameter("MemberID", data.MemberID));
                }

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開數據庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();

                        // 填充 DataSet
                        adapter.Fill(dataSet);

                        // 將查詢結果轉換為 MemberInfo 對象列表
                        return BindingMemberInfo(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal List<MemberInfo> BindingMemberInfo(DataTable dt)
        {
            List<MemberInfo> members = new List<MemberInfo>();
            foreach (DataRow row in dt.Rows)
            {
                // 從每一行數據創建 MemberInfo 對象並添加到列表中
                MemberInfo member = new MemberInfo();
                member.MemberID = Convert.ToInt32(row["MemberID"]);
                member.Email = row["Email"].ToString();
                member.NickName = row["NickName"].ToString();
                member.MemberName = row["MemberName"].ToString();
                member.Birth = Convert.ToDateTime(row["Birth"]);
                member.Phone = row["Phone"].ToString();
                member.Address = row["Address"].ToString();
                member.Gender = row["Gender"].ToString();
                member.RegisterDate = Convert.ToDateTime(row["RegisterDate"]);
                member.Password = row["Password"].ToString();
                member.LastLoginDate = Convert.ToDateTime(row["LastLoginDate"]);
                member.Grade = row["Grade"].ToString();
                member.Point = Convert.ToInt32(row["Point"]);
                member.UpdateUser = row["UpdateUser"].ToString();
                member.UpdateTime = Convert.ToDateTime(row["UpdateTime"]);
                member.Status = row["Status"].ToString();
                member.PhotoPath = row["PhotoPath"].ToString();

                members.Add(member);
            }
            return members;
        }
        internal void UpdateMemberInfo(MemberInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();

                string sqlQuery = "Update MemberInfo set ";

                // SQL 查詢語句
                if (info.Process == MemberInfoProcess.UpdateInfo)
                {
                    sqlQuery += " email=@email, nickname=@nickname, MemberName=@name, birth=@birth, phone=@phone, address=@address, gender=@gender,photoPath=@photoPath where memberId=@memberId ";
                    pars.Add(new SqlParameter("email", info.Email));
                    pars.Add(new SqlParameter("nickname", info.NickName));
                    pars.Add(new SqlParameter("name", info.MemberName));
                    pars.Add(new SqlParameter("birth", info.Birth));
                    pars.Add(new SqlParameter("phone", info.Phone));
                    pars.Add(new SqlParameter("address", info.Address));
                    pars.Add(new SqlParameter("gender", info.Gender));
                    pars.Add(new SqlParameter("memberId", info.MemberID));
                    pars.Add(new SqlParameter("photoPath", info.PhotoPath));
                }
                else if (info.Process == MemberInfoProcess.UpdatePwd)
                {
                    sqlQuery += @" password=@password where memberId=@memberId  ";
                    pars.Add(new SqlParameter("password", info.Password));
                    pars.Add(new SqlParameter("memberId", info.MemberID));
                }
                else if (info.Process == MemberInfoProcess.UpdateStatus)
                {
                    sqlQuery += @" status=@status where memberId=@memberId  ";
                    pars.Add(new SqlParameter("status", info.Status));
                    pars.Add(new SqlParameter("memberId", info.MemberID));
                }

                // 使用 SqlCommand 對象執行 SQL 查詢
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // 打開數據庫連接
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

