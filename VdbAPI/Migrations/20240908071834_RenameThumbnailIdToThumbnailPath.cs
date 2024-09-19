using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VdbAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameThumbnailIdToThumbnailPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {  
            migrationBuilder.CreateTable(
                name: "Cinema",
                columns: table => new
                {
                    CinemaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CinemaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CinemaAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CinemaPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema", x => x.CinemaID);
                });

            migrationBuilder.CreateTable(
                name: "CouponInfo",
                columns: table => new
                {
                    CouponID = table.Column<int>(type: "int", nullable: false, comment: "優惠券編號"),
                    CouponName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "優惠券名稱"),
                    CouponDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "優惠券說明"),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "類型(現金券折價,贈品)"),
                    DiscountFormula = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "折價公式(ex:折50寫N-50)"),
                    GiftListID = table.Column<int>(type: "int", nullable: true, comment: "增品清單編號"),
                    ExpireDate = table.Column<int>(type: "int", nullable: true, comment: "效期(N天)"),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "備註"),
                    CreTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "建立時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponInfo", x => x.CouponID);
                });

            migrationBuilder.CreateTable(
                name: "DirectorList",
                columns: table => new
                {
                    DirectorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DirectorImage = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_導演列表", x => x.DirectorID);
                });

            migrationBuilder.CreateTable(
                name: "GenreList",
                columns: table => new
                {
                    GenreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_類型總表", x => x.GenreID);
                });

            migrationBuilder.CreateTable(
                name: "GiftInfo",
                columns: table => new
                {
                    GiftID = table.Column<int>(type: "int", nullable: false, comment: "贈品編號"),
                    GiftName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "贈品名稱"),
                    GiftDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "贈品說明"),
                    Qty = table.Column<int>(type: "int", nullable: false, comment: "贈品數量"),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "贈品圖片")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftInfo", x => x.GiftID);
                });

            migrationBuilder.CreateTable(
                name: "ImageList",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_圖片列表", x => x.ImageID);
                });

            migrationBuilder.CreateTable(
                name: "KeywordList",
                columns: table => new
                {
                    KeywordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordList", x => x.KeywordID);
                });

            migrationBuilder.CreateTable(
                name: "MemberInfo",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員編號"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "信箱"),
                    NickName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "暱稱"),
                    MemberName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "會員姓名"),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false, comment: "生日"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "手機"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "地址"),
                    Gender = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "性別"),
                    RegisterDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "註冊時間"),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "密碼"),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最後登入時間"),
                    Grade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "會員等級"),
                    Point = table.Column<int>(type: "int", nullable: false, comment: "會員積分"),
                    UpdateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "更新人員或系統功能"),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "更新時間(會員到期,降級,升級由系統功能或者後台更改)"),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banned = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    MemberIdentity = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInformation", x => x.MemberID);
                });

            migrationBuilder.CreateTable(
                name: "PlanList",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanList", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "PlayList",
                columns: table => new
                {
                    PlayListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlayListDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    AddedCount = table.Column<int>(type: "int", nullable: false),
                    SharedCount = table.Column<int>(type: "int", nullable: false),
                    PlayListImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PlayListCreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    PlayListUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    AnalysisTimestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlayList__38709FBBFCBDB92F", x => x.PlayListID);
                });

            migrationBuilder.CreateTable(
                name: "ProductList",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductPrice = table.Column<decimal>(type: "money", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductImg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductList", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "SeriesList",
                columns: table => new
                {
                    SeriesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_系列總表", x => x.SeriesID);
                });

            migrationBuilder.CreateTable(
                name: "StreamingPlatformList",
                columns: table => new
                {
                    PlatformID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingPlatformList", x => x.PlatformID);
                });

            migrationBuilder.CreateTable(
                name: "TheaterList",
                columns: table => new
                {
                    TheaterID = table.Column<int>(type: "int", nullable: false),
                    TheaterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheaterList", x => x.TheaterID);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
                columns: table => new
                {
                    ThemeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MovieDis__F9646BD21F408451", x => x.ThemeID);
                });

            migrationBuilder.CreateTable(
                name: "TypeList",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeList", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfTicket",
                columns: table => new
                {
                    TypeOfTicket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfTicketName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfTicket", x => x.TypeOfTicket);
                });

            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    HallsID = table.Column<int>(type: "int", nullable: false),
                    CinemaID = table.Column<int>(type: "int", nullable: true),
                    HallsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeatCapacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.HallsID);
                    table.ForeignKey(
                        name: "FK_Hall_Cinema",
                        column: x => x.CinemaID,
                        principalTable: "Cinema",
                        principalColumn: "CinemaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiftList",
                columns: table => new
                {
                    GiftListID = table.Column<int>(type: "int", nullable: false, comment: "贈品清單編號"),
                    GiftID = table.Column<int>(type: "int", nullable: false, comment: "贈品編號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftList_1", x => new { x.GiftListID, x.GiftID });
                    table.ForeignKey(
                        name: "FK_GiftList_GiftInfo",
                        column: x => x.GiftID,
                        principalTable: "GiftInfo",
                        principalColumn: "GiftID");
                });

            migrationBuilder.CreateTable(
                name: "BlackList",
                columns: table => new
                {
                    BlacklistID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    BlockedMemberID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blacklis__AFDBF438F52A4D71", x => x.BlacklistID);
                    table.ForeignKey(
                        name: "FK_Blacklist_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Blacklist_MemberInfo1",
                        column: x => x.BlockedMemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "FriendChat",
                columns: table => new
                {
                    FriendChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    ReceiverID = table.Column<int>(type: "int", nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FriendChat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendChat_1", x => x.FriendChatID);
                    table.ForeignKey(
                        name: "FK_FriendChat_MemberInfo",
                        column: x => x.SenderID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_FriendChat_MemberInfo1",
                        column: x => x.ReceiverID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "FriendList",
                columns: table => new
                {
                    FriendListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    FriendID = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FriendStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FriendLi__64B949EBFF0464DA", x => x.FriendListID);
                    table.ForeignKey(
                        name: "FK_FriendList_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_FriendList_MemberInfo1",
                        column: x => x.FriendID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "Invite",
                columns: table => new
                {
                    InvitelID = table.Column<int>(type: "int", nullable: false, comment: "流水號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員ID"),
                    InvitedMemberID = table.Column<int>(type: "int", nullable: false, comment: "受邀好友ID"),
                    InviteTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "邀請時間"),
                    InviteMsg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "邀請訊息"),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "狀態代碼:\r\nInitial初始(I)\r\nAccept接受(A)\r\nReject拒絕(R)\r\nPedding忽略(P)"),
                    StatusTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "狀態時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.InvitelID);
                    table.ForeignKey(
                        name: "FK_Invite_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Invite_MemberInfo1",
                        column: x => x.InvitedMemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "MemberCoupon",
                columns: table => new
                {
                    SerialNo = table.Column<int>(type: "int", nullable: false, comment: "優惠券序號(代碼)"),
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員編號"),
                    CouponID = table.Column<int>(type: "int", nullable: false, comment: "優惠券編號"),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "狀態(是否兌換,是否失效)"),
                    ExpireTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "失效時間"),
                    UseTime = table.Column<DateTime>(type: "datetime", nullable: true, comment: "使用時間"),
                    GetTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "得到優惠券時間"),
                    ActionType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "使用類型(線上電影票/線下電影票)"),
                    ActionRefNo = table.Column<int>(type: "int", nullable: true, comment: "使用的交易單號"),
                    GiftID = table.Column<int>(type: "int", nullable: true, comment: "兌換的贈品編號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCoupon", x => x.SerialNo);
                    table.ForeignKey(
                        name: "FK_MemberCoupon_CouponInfo",
                        column: x => x.CouponID,
                        principalTable: "CouponInfo",
                        principalColumn: "CouponID");
                    table.ForeignKey(
                        name: "FK_MemberCoupon_GiftInfo",
                        column: x => x.GiftID,
                        principalTable: "GiftInfo",
                        principalColumn: "GiftID");
                    table.ForeignKey(
                        name: "FK_MemberCoupon_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "MemberNotice",
                columns: table => new
                {
                    MemberNoticeID = table.Column<int>(type: "int", nullable: false, comment: "會員通知編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員編號"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "通知標題"),
                    NoticeContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "通知內容"),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "狀態:已讀,未讀"),
                    RefNo = table.Column<int>(type: "int", nullable: true, comment: "通知參照的單號"),
                    CreTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "通知時間"),
                    Action = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "建立通知的事件")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberNotice", x => x.MemberNoticeID);
                    table.ForeignKey(
                        name: "FK_MemberNotice_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "PointHistory",
                columns: table => new
                {
                    PointHistoryID = table.Column<int>(type: "int", nullable: false, comment: "積分歷史編號"),
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員編號"),
                    Action = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "積分事件(發文,交易,邀請好友)"),
                    CreTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "事件時間"),
                    RefNo = table.Column<int>(type: "int", nullable: true, comment: "參照事件編號(發文,交易,邀請好友)"),
                    Point = table.Column<int>(type: "int", nullable: false, comment: "積分")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointHistory", x => x.PointHistoryID);
                    table.ForeignKey(
                        name: "FK_PointHistory_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "ReservationDetail",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    CouponID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDetail", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_ReservationDetail_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "ValidCode",
                columns: table => new
                {
                    ValidCodeID = table.Column<int>(type: "int", nullable: false, comment: "流水號"),
                    MemberID = table.Column<int>(type: "int", nullable: false, comment: "會員編號"),
                    ValidCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "驗證碼"),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "驗證類型(忘記密碼,註冊驗證)"),
                    Status = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "狀態"),
                    ExpireTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "到期時間"),
                    CreTime = table.Column<DateTime>(type: "datetime", nullable: false, comment: "建立時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidCode", x => x.ValidCodeID);
                    table.ForeignKey(
                        name: "FK_ValidCode_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "MemberCreatedPlayList",
                columns: table => new
                {
                    MemberCreatedPlayListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PlayListID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCreatedPlayList", x => x.MemberCreatedPlayListID);
                    table.ForeignKey(
                        name: "FK_MemberCreatedPlayList_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_MemberCreatedPlayList_PlayList",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "PlayListID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberPlayList",
                columns: table => new
                {
                    MemberPlayListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PlayListID = table.Column<int>(type: "int", nullable: false),
                    AddedOtherMemberPlayListAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MemberPl__F763B3FA32E920B0", x => x.MemberPlayListID);
                    table.ForeignKey(
                        name: "FK_MemberPlayList_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_MemberPlayList_PlayList",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "PlayListID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayListCollaborator",
                columns: table => new
                {
                    CollaboratorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    CollaboratorJoinedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CollaboratorActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionTimestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlayList__3AC201C3ED5C66AE", x => x.CollaboratorID);
                    table.ForeignKey(
                        name: "FK_PlayListCollaborator_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_PlayListCollaborator_PlayList",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "PlayListID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonList",
                columns: table => new
                {
                    SeasonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesID = table.Column<int>(type: "int", nullable: true),
                    SeasonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeasonNumber = table.Column<int>(type: "int", nullable: true),
                    EpisodeCount = table.Column<int>(type: "int", nullable: true),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonList", x => x.SeasonID);
                    table.ForeignKey(
                        name: "FK_SeasonList_SeriesList",
                        column: x => x.SeriesID,
                        principalTable: "SeriesList",
                        principalColumn: "SeriesID");
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeID = table.Column<int>(type: "int", nullable: false),
                    AuthorID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ArticleContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ReplyCount = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Lock = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ArticleImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Article__9C6270C8A7594F7D", x => x.ArticleID);
                    table.ForeignKey(
                        name: "FK_Article_MemberInfo",
                        column: x => x.AuthorID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Article_Theme",
                        column: x => x.ThemeID,
                        principalTable: "Theme",
                        principalColumn: "ThemeID");
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    SeatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallsID = table.Column<int>(type: "int", nullable: true),
                    RowNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeatNumber = table.Column<int>(type: "int", nullable: true),
                    SeatStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.SeatID);
                    table.ForeignKey(
                        name: "FK_Seat_Hall",
                        column: x => x.HallsID,
                        principalTable: "Hall",
                        principalColumn: "HallsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoList",
                columns: table => new
                {
                    VideoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    VideoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeriesID = table.Column<int>(type: "int", nullable: true),
                    MainGenreID = table.Column<int>(type: "int", nullable: false),
                    SeasonID = table.Column<int>(type: "int", nullable: true),
                    Episode = table.Column<int>(type: "int", nullable: true),
                    RunningTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsShowing = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    Popularity = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ThumbnailID = table.Column<int>(type: "int", nullable: true),
                    Lang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Views = table.Column<int>(type: "int", nullable: true),
                    AgeRating = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrailerURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_作品總表", x => x.VideoID);
                    table.ForeignKey(
                        name: "FK_VideoList_GenreList",
                        column: x => x.MainGenreID,
                        principalTable: "GenreList",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_VideoList_SeasonList",
                        column: x => x.SeasonID,
                        principalTable: "SeasonList",
                        principalColumn: "SeasonID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VideoList_SeriesList",
                        column: x => x.SeriesID,
                        principalTable: "SeriesList",
                        principalColumn: "SeriesID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VideoList_TypeList",
                        column: x => x.TypeID,
                        principalTable: "TypeList",
                        principalColumn: "TypeID");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    PosterID = table.Column<int>(type: "int", nullable: false),
                    PostContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Lock = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PostImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Post__AA126038B6A21AA1", x => x.PostID);
                    table.ForeignKey(
                        name: "FK_Post_Article",
                        column: x => x.PostID,
                        principalTable: "Article",
                        principalColumn: "ArticleID");
                    table.ForeignKey(
                        name: "FK_Post_MemberInfo",
                        column: x => x.PosterID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "CastList",
                columns: table => new
                {
                    CastID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoID = table.Column<int>(type: "int", nullable: true),
                    ActorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaringList", x => x.CastID);
                    table.ForeignKey(
                        name: "FK_StaringList_ActorList",
                        column: x => x.ActorID,
                        principalTable: "ActorList",
                        principalColumn: "ActorID");
                    table.ForeignKey(
                        name: "FK_StaringList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorForVideoList",
                columns: table => new
                {
                    SerialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectorID = table.Column<int>(type: "int", nullable: false),
                    VideoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorForVideoList", x => x.SerialID);
                    table.ForeignKey(
                        name: "FK_DirectorForVideoList_DirectorList",
                        column: x => x.DirectorID,
                        principalTable: "DirectorList",
                        principalColumn: "DirectorID");
                    table.ForeignKey(
                        name: "FK_DirectorForVideoList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenresForVideoList",
                columns: table => new
                {
                    SerialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoID = table.Column<int>(type: "int", nullable: true),
                    GenreID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenresForVideoList", x => x.SerialID);
                    table.ForeignKey(
                        name: "FK_GenresForVideoList_GenreList",
                        column: x => x.GenreID,
                        principalTable: "GenreList",
                        principalColumn: "GenreID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenresForVideoList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageForVideoList",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageID = table.Column<int>(type: "int", nullable: true),
                    VideoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageForWorkList", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ImageForVideoList_ImageList",
                        column: x => x.ImageID,
                        principalTable: "ImageList",
                        principalColumn: "ImageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageForVideoList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordForVideoList",
                columns: table => new
                {
                    SerialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeywordID = table.Column<int>(type: "int", nullable: true),
                    VideoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordFroVideoList", x => x.SerialID);
                    table.ForeignKey(
                        name: "FK_KeywordFroVideoList_KeywordList",
                        column: x => x.KeywordID,
                        principalTable: "KeywordList",
                        principalColumn: "KeywordID");
                    table.ForeignKey(
                        name: "FK_KeywordFroVideoList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NowShowingTheater",
                columns: table => new
                {
                    CombinationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CinemaID = table.Column<int>(type: "int", nullable: true),
                    VideoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NowShowingTheater", x => x.CombinationID);
                    table.ForeignKey(
                        name: "FK_NowShowingTheater_Cinema",
                        column: x => x.CinemaID,
                        principalTable: "Cinema",
                        principalColumn: "CinemaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NowShowingTheater_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayListItem",
                columns: table => new
                {
                    PlayListItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListID = table.Column<int>(type: "int", nullable: false),
                    VideoID = table.Column<int>(type: "int", nullable: false),
                    VideoPosition = table.Column<int>(type: "int", nullable: false),
                    VideoAddedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlayList__5B7B5B943F02F781", x => x.PlayListItemID);
                    table.ForeignKey(
                        name: "FK_PlayListItem_PlayList",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "PlayListID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayListItem_VideoList1",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID");
                });

            migrationBuilder.CreateTable(
                name: "VideoStreamingLinkList",
                columns: table => new
                {
                    StreamingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformID = table.Column<int>(type: "int", nullable: false),
                    StreamLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VideoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoStreamingLinkList", x => x.StreamingID);
                    table.ForeignKey(
                        name: "FK_VideoStreamingLinkList_StreamingPlatformList",
                        column: x => x.PlatformID,
                        principalTable: "StreamingPlatformList",
                        principalColumn: "PlatformID");
                    table.ForeignKey(
                        name: "FK_VideoStreamingLinkList_VideoList",
                        column: x => x.VideoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViedoPlanList",
                columns: table => new
                {
                    ViedoPlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    ViedoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViedoPlanList", x => x.ViedoPlanID);
                    table.ForeignKey(
                        name: "FK_ViedoPlanList_PlanList",
                        column: x => x.PlanID,
                        principalTable: "PlanList",
                        principalColumn: "PlanID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViedoPlanList_VideoList",
                        column: x => x.ViedoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID");
                });

            migrationBuilder.CreateTable(
                name: "ShowingHall",
                columns: table => new
                {
                    ShowingHallID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CombinationID = table.Column<int>(type: "int", nullable: false),
                    HallsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowingHall", x => x.ShowingHallID);
                    table.ForeignKey(
                        name: "FK_ShowingHall_Hall",
                        column: x => x.HallsID,
                        principalTable: "Hall",
                        principalColumn: "HallsID");
                    table.ForeignKey(
                        name: "FK_ShowingHall_NowShowingTheater",
                        column: x => x.CombinationID,
                        principalTable: "NowShowingTheater",
                        principalColumn: "CombinationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    ShoppingCartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ViedoPlanID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.ShoppingCartID);
                    table.ForeignKey(
                        name: "FK_ShoppingCart_MemberInfo",
                        column: x => x.MemberID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_ShoppingCart_ViedoPlanList",
                        column: x => x.ViedoPlanID,
                        principalTable: "ViedoPlanList",
                        principalColumn: "ViedoPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showtime",
                columns: table => new
                {
                    ShowtimeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViedoID = table.Column<int>(type: "int", nullable: true),
                    HallsID = table.Column<int>(type: "int", nullable: true),
                    ShowTimeDATETIME = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtime", x => x.ShowtimeID);
                    table.ForeignKey(
                        name: "FK_Showtime_Hall",
                        column: x => x.HallsID,
                        principalTable: "Hall",
                        principalColumn: "HallsID");
                    table.ForeignKey(
                        name: "FK_Showtime_ShowingHall",
                        column: x => x.HallsID,
                        principalTable: "ShowingHall",
                        principalColumn: "ShowingHallID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Showtime_VideoList",
                        column: x => x.ViedoID,
                        principalTable: "VideoList",
                        principalColumn: "VideoID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartID = table.Column<int>(type: "int", nullable: true),
                    CouponID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OrderTotalPrice = table.Column<decimal>(type: "money", nullable: true),
                    DeliveryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    DriverID = table.Column<int>(type: "int", nullable: true),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: true),
                    LastEditTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_CouponInfo",
                        column: x => x.CouponID,
                        principalTable: "CouponInfo",
                        principalColumn: "CouponID");
                    table.ForeignKey(
                        name: "FK_Order_MemberInfo",
                        column: x => x.DriverID,
                        principalTable: "MemberInfo",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Order_ShoppingCart",
                        column: x => x.ShoppingCartID,
                        principalTable: "ShoppingCart",
                        principalColumn: "ShoppingCartID");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    TypeOfTicket = table.Column<int>(type: "int", nullable: true),
                    ShowtimeID = table.Column<int>(type: "int", nullable: true),
                    SeatID = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_Ticket_Seat1",
                        column: x => x.SeatID,
                        principalTable: "Seat",
                        principalColumn: "SeatID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Ticket_Showtime",
                        column: x => x.ShowtimeID,
                        principalTable: "Showtime",
                        principalColumn: "ShowtimeID");
                    table.ForeignKey(
                        name: "FK_Ticket_TypeOfTicket",
                        column: x => x.TypeOfTicket,
                        principalTable: "TypeOfTicket",
                        principalColumn: "TypeOfTicket");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_ProductList",
                        column: x => x.ProductID,
                        principalTable: "ProductList",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuthorID",
                table: "Article",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Article_ThemeID",
                table: "Article",
                column: "ThemeID");

            migrationBuilder.CreateIndex(
                name: "NonClusteredIndex-20240815-155924",
                table: "Article",
                columns: new[] { "ArticleID", "ThemeID", "AuthorID", "Title", "PostDate", "Lock" });

            migrationBuilder.CreateIndex(
                name: "IX_BlackList_BlockedMemberID",
                table: "BlackList",
                column: "BlockedMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_BlackList_MemberID",
                table: "BlackList",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_CastList_ActorID",
                table: "CastList",
                column: "ActorID");

            migrationBuilder.CreateIndex(
                name: "IX_CastList_VideoID",
                table: "CastList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorForVideoList_DirectorID",
                table: "DirectorForVideoList",
                column: "DirectorID");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorForVideoList_VideoID",
                table: "DirectorForVideoList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendChat_ReceiverID",
                table: "FriendChat",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendChat_SenderID",
                table: "FriendChat",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_FriendID",
                table: "FriendList",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_MemberID",
                table: "FriendList",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_GenresForVideoList_GenreID",
                table: "GenresForVideoList",
                column: "GenreID");

            migrationBuilder.CreateIndex(
                name: "IX_GenresForVideoList_VideoID",
                table: "GenresForVideoList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_GiftList_GiftID",
                table: "GiftList",
                column: "GiftID");

            migrationBuilder.CreateIndex(
                name: "IX_Hall_CinemaID",
                table: "Hall",
                column: "CinemaID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageForVideoList_ImageID",
                table: "ImageForVideoList",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageForVideoList_VideoID",
                table: "ImageForVideoList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_InvitedMemberID",
                table: "Invite",
                column: "InvitedMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_MemberID",
                table: "Invite",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordForVideoList_KeywordID",
                table: "KeywordForVideoList",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordForVideoList_VideoID",
                table: "KeywordForVideoList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCoupon_CouponID",
                table: "MemberCoupon",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCoupon_GiftID",
                table: "MemberCoupon",
                column: "GiftID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCoupon_MemberID",
                table: "MemberCoupon",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCreatedPlayList_MemberID",
                table: "MemberCreatedPlayList",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCreatedPlayList_PlayListID",
                table: "MemberCreatedPlayList",
                column: "PlayListID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberNotice_MemberID",
                table: "MemberNotice",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPlayList_MemberID",
                table: "MemberPlayList",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPlayList_PlayListID",
                table: "MemberPlayList",
                column: "PlayListID");

            migrationBuilder.CreateIndex(
                name: "IX_NowShowingTheater_CinemaID",
                table: "NowShowingTheater",
                column: "CinemaID");

            migrationBuilder.CreateIndex(
                name: "IX_NowShowingTheater_VideoID",
                table: "NowShowingTheater",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CouponID",
                table: "Order",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DriverID",
                table: "Order",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShoppingCartID",
                table: "Order",
                column: "ShoppingCartID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderID",
                table: "OrderDetail",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductID",
                table: "OrderDetail",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListCollaborator_MemberID",
                table: "PlayListCollaborator",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListCollaborator_PlayListID",
                table: "PlayListCollaborator",
                column: "PlayListID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListItem_PlayListID",
                table: "PlayListItem",
                column: "PlayListID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListItem_VideoID",
                table: "PlayListItem",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_PointHistory_MemberID",
                table: "PointHistory",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PosterID",
                table: "Post",
                column: "PosterID");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetail_MemberID",
                table: "ReservationDetail",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonList_SeriesID",
                table: "SeasonList",
                column: "SeriesID");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_HallsID",
                table: "Seat",
                column: "HallsID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_MemberID",
                table: "ShoppingCart",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_ViedoPlanID",
                table: "ShoppingCart",
                column: "ViedoPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_ShowingHall_CombinationID",
                table: "ShowingHall",
                column: "CombinationID");

            migrationBuilder.CreateIndex(
                name: "IX_ShowingHall_HallsID",
                table: "ShowingHall",
                column: "HallsID");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_HallsID",
                table: "Showtime",
                column: "HallsID");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_ViedoID",
                table: "Showtime",
                column: "ViedoID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_SeatID",
                table: "Ticket",
                column: "SeatID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ShowtimeID",
                table: "Ticket",
                column: "ShowtimeID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TypeOfTicket",
                table: "Ticket",
                column: "TypeOfTicket");

            migrationBuilder.CreateIndex(
                name: "IX_ValidCode_MemberID",
                table: "ValidCode",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoList_MainGenreID",
                table: "VideoList",
                column: "MainGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoList_SeasonID",
                table: "VideoList",
                column: "SeasonID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoList_SeriesID",
                table: "VideoList",
                column: "SeriesID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoList_TypeID",
                table: "VideoList",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStreamingLinkList_PlatformID",
                table: "VideoStreamingLinkList",
                column: "PlatformID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoStreamingLinkList_VideoID",
                table: "VideoStreamingLinkList",
                column: "VideoID");

            migrationBuilder.CreateIndex(
                name: "IX_ViedoPlanList_PlanID",
                table: "ViedoPlanList",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_ViedoPlanList_ViedoID",
                table: "ViedoPlanList",
                column: "ViedoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackList");

            migrationBuilder.DropTable(
                name: "CastList");

            migrationBuilder.DropTable(
                name: "DirectorForVideoList");

            migrationBuilder.DropTable(
                name: "FriendChat");

            migrationBuilder.DropTable(
                name: "FriendList");

            migrationBuilder.DropTable(
                name: "GenresForVideoList");

            migrationBuilder.DropTable(
                name: "GiftList");

            migrationBuilder.DropTable(
                name: "ImageForVideoList");

            migrationBuilder.DropTable(
                name: "Invite");

            migrationBuilder.DropTable(
                name: "KeywordForVideoList");

            migrationBuilder.DropTable(
                name: "MemberCoupon");

            migrationBuilder.DropTable(
                name: "MemberCreatedPlayList");

            migrationBuilder.DropTable(
                name: "MemberNotice");

            migrationBuilder.DropTable(
                name: "MemberPlayList");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "PlayListCollaborator");

            migrationBuilder.DropTable(
                name: "PlayListItem");

            migrationBuilder.DropTable(
                name: "PointHistory");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "ReservationDetail");

            migrationBuilder.DropTable(
                name: "TheaterList");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "ValidCode");

            migrationBuilder.DropTable(
                name: "VideoStreamingLinkList");

            migrationBuilder.DropTable(
                name: "ActorList");

            migrationBuilder.DropTable(
                name: "DirectorList");

            migrationBuilder.DropTable(
                name: "ImageList");

            migrationBuilder.DropTable(
                name: "KeywordList");

            migrationBuilder.DropTable(
                name: "GiftInfo");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductList");

            migrationBuilder.DropTable(
                name: "PlayList");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Showtime");

            migrationBuilder.DropTable(
                name: "TypeOfTicket");

            migrationBuilder.DropTable(
                name: "StreamingPlatformList");

            migrationBuilder.DropTable(
                name: "CouponInfo");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "Theme");

            migrationBuilder.DropTable(
                name: "ShowingHall");

            migrationBuilder.DropTable(
                name: "MemberInfo");

            migrationBuilder.DropTable(
                name: "ViedoPlanList");

            migrationBuilder.DropTable(
                name: "Hall");

            migrationBuilder.DropTable(
                name: "NowShowingTheater");

            migrationBuilder.DropTable(
                name: "PlanList");

            migrationBuilder.DropTable(
                name: "Cinema");

            migrationBuilder.DropTable(
                name: "VideoList");

            migrationBuilder.DropTable(
                name: "GenreList");

            migrationBuilder.DropTable(
                name: "SeasonList");

            migrationBuilder.DropTable(
                name: "TypeList");

            migrationBuilder.DropTable(
                name: "SeriesList");
        }
    }
}
