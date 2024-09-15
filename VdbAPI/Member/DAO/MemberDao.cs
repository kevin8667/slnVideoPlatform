using VdbAPI.Member.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VdbAPI.Member.Model.mMemberInfo;
using VdbAPI.Member.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace VdbAPI.Member.Dao
{
    internal class MemberDao
    {
        private string _connectionString { get; set; }
        internal MemberDao(string connection)
        {
            _connectionString = connection;
        }
        internal void SetNoticeMessage(string memberid, string title, string content, string action)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"INSERT INTO MemberNotice ( MemberID, Title, NoticeContent, Status, CreTime, Action )
               VALUES ( @MemberID, @Title, @NoticeContent, 'N', GETDATE(), @Action ) ";
                List<SqlParameter> pars = new List<SqlParameter>();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@MemberID", memberid);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@NoticeContent", content);
                    command.Parameters.AddWithValue("@Action", action);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteNoticeMessage(int memberNoticeID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string deleteQuery = @"DELETE FROM MemberNotice WHERE MemberNoticeID = @MemberNoticeID";

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@MemberNoticeID", memberNoticeID);
                    connection.Open();
                    int rowsAffected = deleteCommand.ExecuteNonQuery(); // 执行删除

                    if (rowsAffected == 0)
                    {
                        // 处理没有删除任何行的情况（可选）
                    }
                }
            }
        }


        internal List<mMemberNotice> GetMemberNotice(mMemberNotice data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM MemberNotice where 1=1 ";
                List<SqlParameter> pars = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(data.Title))
                {
                    sqlQuery += @" and Title = @Title ";
                    pars.Add(new SqlParameter("Title", data.Title));
                }
                if (data.MemberID != null)
                {
                    sqlQuery += @" and MemberID = @MemberID ";
                    pars.Add(new SqlParameter("MemberID", data.MemberID));
                }
                if (data.MemberNoticeID != null)
                {
                    sqlQuery += @" and MemberNoticeID = @MemberNoticeID ";
                    pars.Add(new SqlParameter("MemberNoticeID", data.MemberNoticeID));
                }
                if (!string.IsNullOrEmpty(data.Action))
                {
                    sqlQuery += @" and Action = @Action ";
                    pars.Add(new SqlParameter("Action", data.Action));
                }
                if (!string.IsNullOrEmpty(data.Status))
                {
                    sqlQuery += @" and Status = @Status ";
                    pars.Add(new SqlParameter("Status", data.Status));
                }
                if (!string.IsNullOrEmpty(data.NoticeContent))
                {
                    sqlQuery += @" and NoticeContent = @NoticeContent ";
                    pars.Add(new SqlParameter("NoticeContent", data.NoticeContent));
                }
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();


                        adapter.Fill(dataSet);


                        return BindingMemberNotice(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal void UpdateMemberPic(mMemberInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE MemberInfo SET PhotoPath = @PhotoPath WHERE MemberID = @MemberID ";
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@PhotoPath", data.PhotoPath));
                pars.Add(new SqlParameter("@MemberID", data.MemberID));

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
        internal List<mMemberInfo> GetMemberData(mMemberInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM MemberInfo where 1=1 ";
                List<SqlParameter> pars = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(data.MemberName))
                {
                    sqlQuery += @" and MemberName like @MemberName ";
                    pars.Add(new SqlParameter("MemberName", "%" + data.MemberName + "%"));
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
                if (!string.IsNullOrEmpty(data.PhotoPath))
                {
                    sqlQuery += @" and PhotoPath = @PhotoPath ";
                    pars.Add(new SqlParameter("PhotoPath", data.PhotoPath));
                }
                if (!string.IsNullOrEmpty(data.NickName))
                {
                    sqlQuery += @" and NickName = @NickName ";
                    pars.Add(new SqlParameter("NickName", data.NickName));
                }
                if (data.Birth >= DateTime.Now.AddYears(-120) && data.Birth <= DateTime.Now)
                {
                    sqlQuery += @" and Birth = @Birth ";
                    pars.Add(new SqlParameter("Birth", data.Birth));
                }
                if (!string.IsNullOrEmpty(data.Address))
                {
                    sqlQuery += @" and Address like @Address ";
                    pars.Add(new SqlParameter("Address", "%" + data.Address + "%"));
                }
                if (!string.IsNullOrEmpty(data.Gender))
                {
                    sqlQuery += @" and Gender = @Gender ";
                    pars.Add(new SqlParameter("Gender", data.Gender));
                }
                if (data.Point != null)
                {
                    sqlQuery += @" and Point = @Point ";
                    pars.Add(new SqlParameter("Point", data.Point));
                }
                if (!string.IsNullOrEmpty(data.Grade))
                {
                    sqlQuery += @" and Grade = @Grade ";
                    pars.Add(new SqlParameter("Grade", data.Grade));
                }
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();


                        adapter.Fill(dataSet);

                        return BindingMemberInfo(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal void InviteFriend(int memberId, int friendId, string message, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"
            INSERT INTO FriendList ( MemberID, FriendID, CreationDate, FriendStatus, InvitedMessage)
values
            (@MemberID,@FriendID,CONVERT(DATE, GETDATE()),@status,@InvitedMessage)";

                List<SqlParameter> pars = new List<SqlParameter>
        {
            new SqlParameter("@MemberID", memberId),
            new SqlParameter("@FriendID", friendId),
            new SqlParameter("@status", status),
            new SqlParameter("@InvitedMessage", message)
        };

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddRange(pars.ToArray());
                    command.ExecuteNonQuery();
                }
            }
        }
        internal mMemberInfo GetEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT * FROM MemberInfo WHERE Email = @Email";

                SqlParameter parameter = new SqlParameter("@Email", email);

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.Add(parameter);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // 創建 mMemberInfo 對象並填充數據
                        mMemberInfo memberInfo = new mMemberInfo
                        {
                            Email = reader["Email"].ToString(),
                            // 填充其他屬性
                        };

                        return memberInfo;
                    }
                    else
                    {
                        return null; // 未找到匹配的電子郵件地址
                    }
                }
            }
        }





        internal List<memberFriends> GetFriendList(int memberId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT F.FriendID, CONVERT(DATE, F.CreationDate) AS CreationDate, F.FriendStatus, F.InvitedMessage, 
                                    M.NickName, M.MemberName, M.PhotoPath, M.MemberID 
                                    FROM FriendList F 
                                    JOIN MemberInfo M ON F.FriendID = M.MemberID
                                    WHERE F.MemberID = @MemberID";

                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@MemberID", memberId)
                };

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        return BindingFriendList(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal void AddFriend(int memberId, int friendId, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = @"
           update friendlist set friendstatus=@status where MemberId=@MemberID and FriendID=@FriendID";

                List<SqlParameter> pars = new List<SqlParameter>
        {
            new SqlParameter("@MemberID", memberId),
            new SqlParameter("@FriendID", friendId),
            new SqlParameter("@status", status)
        };

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddRange(pars.ToArray());
                    command.ExecuteNonQuery();
                }
            }
        }
        internal List<memberFriends> BindingFriendList(DataTable dt)
        {
            List<memberFriends> mFriends = new List<memberFriends>();
            foreach (DataRow row in dt.Rows)
            {
                memberFriends mfriend = new memberFriends();
                mfriend.InvitedMessage = row["InvitedMessage"].ToString();
                mfriend.FriendStatus = row["FriendStatus"].ToString();
                mfriend.FriendId = Convert.ToInt32(row["FriendID"]);
                mfriend.CreationDate = Convert.ToDateTime(row["CreationDate"]);
                mfriend.MemberID = Convert.ToInt32(row["MemberID"]);
                mfriend.NickName = row["NickName"].ToString();
                mfriend.MemberName = row["MemberName"].ToString();
                mfriend.PhotoPath = row["PhotoPath"].ToString();

                mFriends.Add(mfriend);
            }
            return mFriends;
        }
        internal List<mMemberNotice> BindingMemberNotice(DataTable dt)
        {
            List<mMemberNotice> members = new List<mMemberNotice>();
            foreach (DataRow row in dt.Rows)
            {
                mMemberNotice member = new mMemberNotice();
                member.NoticeContent = row["NoticeContent"].ToString();
                member.Title = row["Title"].ToString();
                member.Status = row["Status"].ToString();
                member.Action = row["Action"].ToString();
                member.MemberNoticeID = Convert.ToInt32(row["MemberNoticeID"]);
                member.CreTime = Convert.ToDateTime(row["CreTime"]);
                member.MemberID = Convert.ToInt32(row["MemberID"]);

                members.Add(member);
            }
            return members;
        }
        internal List<mMemberInfo> BindingMemberInfo(DataTable dt)
        {
            List<mMemberInfo> members = new List<mMemberInfo>();
            foreach (DataRow row in dt.Rows)
            {

                mMemberInfo member = new mMemberInfo();
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
                member.Grade = row["Grade"].ToString();
                member.Point = Convert.ToInt32(row["Point"]);
                member.Status = row["Status"].ToString();
                member.PhotoPath = row["PhotoPath"].ToString();
                member.LineUserId = row["LineUserId"].ToString();
                member.BindingLine = row["BindingLine"].ToString();
                members.Add(member);
            }
            return members;
        }
        internal void UpdatePWD(string email, string newPwd)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();

                string sqlQuery = "Update MemberInfo set ";
                {
                    sqlQuery += @" password=@password where email=@email  ";
                    pars.Add(new SqlParameter("password", newPwd));
                    pars.Add(new SqlParameter("email", email));
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
        internal List<mMemberInfo> SelectMemberInfo(mMemberInfo data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

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


                if (!string.IsNullOrEmpty(data.LineUserId))
                {
                    sqlQuery += @" and LineUserId = @LineUserId ";
                    pars.Add(new SqlParameter("LineUserId", data.LineUserId));
                }

                if (!string.IsNullOrEmpty(data.BindingLine))
                {
                    sqlQuery += @" and BindingLine = @BindingLine ";
                    pars.Add(new SqlParameter("BindingLine", data.BindingLine));
                }


                if (data.MemberID != null)
                {
                    sqlQuery += @" and MemberID = @MemberID ";
                    pars.Add(new SqlParameter("MemberID", data.MemberID));
                }


                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();


                        adapter.Fill(dataSet);


                        return BindingMemberInfo(dataSet.Tables[0]);
                    }
                }
            }
        }
        internal void InsertMember(RegisterViewModel info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();

                string sqlQuery = @"INSERT INTO dbo.MemberInfo
           (MemberId, Email,NickName,MemberName,Birth,Phone,Address,Gender,RegisterDate,Password,Grade,Point,Status,FIDOEnabled)
     VALUES
           ((select max(memberId)+1 from memberInfo ),@Email,
           @NickName,
           @MemberName,
           @Birth,
           @Phone,
           @Address,
           @Gender,
           getdate(),
           @Password,
'A',
0,
'Y',
0
          )
";
                pars.Add(new SqlParameter("@Email", info.Email));
                pars.Add(new SqlParameter("@NickName", info.NickName));
                pars.Add(new SqlParameter("@MemberName", info.MemberName));
                pars.Add(new SqlParameter("@Birth", info.Birth));
                pars.Add(new SqlParameter("@Phone", info.Phone));
                pars.Add(new SqlParameter("@Address", info.Address));
                pars.Add(new SqlParameter("@Gender", info.Gender));
                pars.Add(new SqlParameter("@Password", info.Password));

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }




        internal void UpdateMemberInfo(mMemberInfo info)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<SqlParameter> pars = new List<SqlParameter>();

                string sqlQuery = "Update MemberInfo set ";


                if (info.Process == MemberInfoProcess.UpdateInfo)
                {
                    sqlQuery += @" email=@email, nickname=@nickname, MemberName=@name, birth=@birth, phone=@phone, 
                    address=@address, gender=@gender,photoPath=@photoPath where memberId=@memberId ";
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
                else if (info.Process == MemberInfoProcess.UpdateLineUser)
                {
                    sqlQuery += @" BindingLine=@BindingLine , LineUserId=@LineUserId where memberId=@memberId";
                    pars.Add(new SqlParameter("BindingLine", info.BindingLine));
                    pars.Add(new SqlParameter("LineUserId", info.LineUserId));
                    pars.Add(new SqlParameter("memberId", info.MemberID));
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    connection.Open();
                    command.Parameters.AddRange(pars.ToArray());

                    command.ExecuteNonQuery();
                }
            }
        }
        internal void DeleteFriend(int memberId, int friendId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"delete FriendList WHERE MemberID = @MemberID AND FriendID = @FriendID";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@MemberID", memberId);
                    command.Parameters.AddWithValue("@FriendID", friendId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

