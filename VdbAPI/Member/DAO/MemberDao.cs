﻿using VdbAPI.Member.Model;
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

namespace VdbAPI.Member.Dao
{
    internal class MemberDao
    {
        private string _connectionString { get; set; }
        internal MemberDao(string connection)
        {
            _connectionString = connection;
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
                if (data.RefNo != null)
                {
                    sqlQuery += @" and RefNo = @RefNo ";
                    pars.Add(new SqlParameter("RefNo", data.RefNo));
                }
                if (data.CreTime <= DateTime.Now)
                {
                    sqlQuery += @" and CreTime = @CreTime ";
                    pars.Add(new SqlParameter("CreTime", data.CreTime));
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
                if (data.LastLoginDate <= DateTime.Now)
                {
                    sqlQuery += @" and LastLoginDate = @LastLoginDate ";
                    pars.Add(new SqlParameter("LastLoginDate", data.LastLoginDate));
                }
                if (data.RegisterDate <= DateTime.Now)
                {
                    sqlQuery += @" and RegisterDate = @RegisterDate ";
                    pars.Add(new SqlParameter("RegisterDate", data.RegisterDate));
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
        internal List<memberFriends> GetFriendList(memberFriends data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                string sqlQuery = @"SELECT F.FriendID, F.CreationDate, F.FriendStatus, F.InvitedMessage, 
                                    M.NickName, M.MemberName, M.PhotoPath, M.MemberID FROM FriendList F , MemberInfo M where 1=1 ";
                List<SqlParameter> pars = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(data.InvitedMessage))
                {
                    sqlQuery += @" and InvitedMessage like @InvitedMessage ";
                    pars.Add(new SqlParameter("InvitedMessage", "%" + data.InvitedMessage + "%"));
                }
                if (!string.IsNullOrEmpty(data.FriendStatus))
                {
                    sqlQuery += @" and FriendStatus = @FriendStatus ";
                    pars.Add(new SqlParameter("FriendStatus", data.FriendStatus));
                }
                if (data.CreationDate <= DateTime.MinValue || data.CreationDate > DateTime.Now)
                {
                    data.CreationDate = new DateTime(2020, 1, 1);
                }

                if (data.CreationDate <= DateTime.Now)
                {
                    sqlQuery += @" and CreationDate = @CreationDate ";
                    pars.Add(new SqlParameter("CreationDate", data.CreationDate));
                }
                if (data.FriendID != null)
                {
                    sqlQuery += @" and FriendID = @FriendID ";
                    pars.Add(new SqlParameter("FriendID", data.FriendID));
                }
                if (data.MemberID != null)
                {
                    sqlQuery += @" and M.MemberID = @MemberID ";
                    pars.Add(new SqlParameter("MemberID", data.MemberID));
                }
                if (!string.IsNullOrEmpty(data.PhotoPath))
                {
                    sqlQuery += @" and PhotoPath = @PhotoPath ";
                    pars.Add(new SqlParameter("PhotoPath", data.PhotoPath));
                }
                if (!string.IsNullOrEmpty(data.MemberName))
                {
                    sqlQuery += @" and MemberName = @MemberName ";
                    pars.Add(new SqlParameter("MemberName", data.MemberName));
                }
                if (!string.IsNullOrEmpty(data.NickName))
                {
                    sqlQuery += @" and NickName = @NickName ";
                    pars.Add(new SqlParameter("NickName", data.NickName));
                }

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
        internal List<memberFriends> BindingFriendList(DataTable dt)
        {
            List<memberFriends> mFriends = new List<memberFriends>();
            foreach (DataRow row in dt.Rows)
            {
                memberFriends mfriend = new memberFriends();
                mfriend.InvitedMessage = row["InvitedMessage"].ToString();
                mfriend.FriendStatus = row["FriendStatus"].ToString();
                mfriend.FriendID = Convert.ToInt32(row["FriendID"]);
                mfriend.CreationDate = Convert.ToDateTime(row["CreationDate"]);
                mfriend.MemberID = Convert.ToInt32(row["MemberID"]);
                mfriend.FriendListID = Convert.ToInt32(row["FriendListID"]);
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
                member.RefNo = Convert.ToInt32(row["RefNo"]);

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

