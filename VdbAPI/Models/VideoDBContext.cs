﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VdbAPI.Models;

public partial class VideoDBContext : DbContext
{
    public VideoDBContext(DbContextOptions<VideoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActorList> ActorLists { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleView> ArticleViews { get; set; }

    public virtual DbSet<BlackList> BlackLists { get; set; }

    public virtual DbSet<CastList> CastLists { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<CouponInfo> CouponInfos { get; set; }

    public virtual DbSet<DirectorForVideoList> DirectorForVideoLists { get; set; }

    public virtual DbSet<DirectorList> DirectorLists { get; set; }

    public virtual DbSet<Fidocredential> Fidocredentials { get; set; }

    public virtual DbSet<FriendList> FriendLists { get; set; }

    public virtual DbSet<GenreList> GenreLists { get; set; }

    public virtual DbSet<GenresForVideoList> GenresForVideoLists { get; set; }

    public virtual DbSet<GiftInfo> GiftInfos { get; set; }

    public virtual DbSet<GiftList> GiftLists { get; set; }

    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<ImageForVideoList> ImageForVideoLists { get; set; }

    public virtual DbSet<ImageList> ImageLists { get; set; }

    public virtual DbSet<Invite> Invites { get; set; }

    public virtual DbSet<KeywordForVideoList> KeywordForVideoLists { get; set; }

    public virtual DbSet<KeywordList> KeywordLists { get; set; }

    public virtual DbSet<MemberCoupon> MemberCoupons { get; set; }

    public virtual DbSet<MemberCreatedPlayList> MemberCreatedPlayLists { get; set; }

    public virtual DbSet<MemberInfo> MemberInfos { get; set; }

    public virtual DbSet<MemberNotice> MemberNotices { get; set; }

    public virtual DbSet<MemberPlayList> MemberPlayLists { get; set; }

    public virtual DbSet<MemberRating> MemberRatings { get; set; }

    public virtual DbSet<NowShowingTheater> NowShowingTheaters { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PlanList> PlanLists { get; set; }

    public virtual DbSet<PlayList> PlayLists { get; set; }

    public virtual DbSet<PlayListCollaborator> PlayListCollaborators { get; set; }

    public virtual DbSet<PlayListItem> PlayListItems { get; set; }

    public virtual DbSet<PointHistory> PointHistories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostUserReaction> PostUserReactions { get; set; }

    public virtual DbSet<ProductList> ProductLists { get; set; }

    public virtual DbSet<ReservationDetail> ReservationDetails { get; set; }

    public virtual DbSet<SeasonList> SeasonLists { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeriesList> SeriesLists { get; set; }

    public virtual DbSet<SessionSeat> SessionSeats { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<ShowingHall> ShowingHalls { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<StreamingPlatformList> StreamingPlatformLists { get; set; }

    public virtual DbSet<TheaterList> TheaterLists { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TypeList> TypeLists { get; set; }

    public virtual DbSet<TypeOfTicket> TypeOfTickets { get; set; }

    public virtual DbSet<UserReaction> UserReactions { get; set; }

    public virtual DbSet<ValidCode> ValidCodes { get; set; }

    public virtual DbSet<VideoList> VideoLists { get; set; }

    public virtual DbSet<VideoStreamingLinkList> VideoStreamingLinkLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

        modelBuilder.Entity<ActorList>(entity =>
        {
            entity.HasKey(e => e.ActorId).HasName("PK_演員列表");

            entity.ToTable("ActorList");

            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.ActorDescription).HasMaxLength(500);
            entity.Property(e => e.ActorImgPath).HasMaxLength(300);
            entity.Property(e => e.ActorName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).IsClustered(false);

            entity.ToTable("Article");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Lock).HasDefaultValue(true);
            entity.Property(e => e.PostDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ArticleView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ArticleView");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.NickName)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.PostDate).HasColumnType("datetime");
            entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
            entity.Property(e => e.ThemeName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<BlackList>(entity =>
        {
            entity.HasKey(e => e.BlacklistId).HasName("PK__Blacklis__AFDBF438F52A4D71");

            entity.ToTable("BlackList");

            entity.Property(e => e.BlacklistId)
                .ValueGeneratedNever()
                .HasColumnName("BlacklistID");
            entity.Property(e => e.BlockedMemberId).HasColumnName("BlockedMemberID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Reason).HasMaxLength(50);
        });

        modelBuilder.Entity<CastList>(entity =>
        {
            entity.HasKey(e => e.CastId).HasName("PK_StaringList");

            entity.ToTable("CastList");

            entity.Property(e => e.CastId).HasColumnName("CastID");
            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.ChatRoomId).IsClustered(false);

            entity.ToTable("ChatRoom");

            entity.Property(e => e.ChatRoomId).HasColumnName("ChatRoomID");
            entity.Property(e => e.ChatMessage)
                .IsRequired()
                .HasMaxLength(2000);
            entity.Property(e => e.SendTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.ToTable("Cinema");

            entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
            entity.Property(e => e.CinemaAddress).HasMaxLength(50);
            entity.Property(e => e.CinemaName).HasMaxLength(50);
            entity.Property(e => e.CinemaPhone).HasMaxLength(50);
        });

        modelBuilder.Entity<CouponInfo>(entity =>
        {
            entity.HasKey(e => e.CouponId);

            entity.ToTable("CouponInfo");

            entity.Property(e => e.CouponId)
                .ValueGeneratedNever()
                .HasComment("優惠券編號")
                .HasColumnName("CouponID");
            entity.Property(e => e.CouponDesc)
                .HasMaxLength(500)
                .HasComment("優惠券說明");
            entity.Property(e => e.CouponName)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("優惠券名稱");
            entity.Property(e => e.CreTime)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountFormula)
                .HasMaxLength(100)
                .HasComment("折價公式(ex:折50寫N-50)");
            entity.Property(e => e.ExpireDate).HasComment("效期(N天)");
            entity.Property(e => e.GiftListId)
                .HasComment("增品清單編號")
                .HasColumnName("GiftListID");
            entity.Property(e => e.Remark)
                .HasMaxLength(100)
                .HasComment("備註");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("類型(現金券折價,贈品)");
        });

        modelBuilder.Entity<DirectorForVideoList>(entity =>
        {
            entity.HasKey(e => e.SerialId);

            entity.ToTable("DirectorForVideoList");

            entity.Property(e => e.SerialId).HasColumnName("SerialID");
            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<DirectorList>(entity =>
        {
            entity.HasKey(e => e.DirectorId).HasName("PK_導演列表");

            entity.ToTable("DirectorList");

            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
            entity.Property(e => e.DirectorDescription).HasMaxLength(500);
            entity.Property(e => e.DirectorImgPath).HasMaxLength(300);
            entity.Property(e => e.DirectorName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Fidocredential>(entity =>
        {
            entity.HasKey(e => e.FidocredentialId).HasName("PK__FIDOCred__935B270A20A53E8C");

            entity.ToTable("FIDOCredential");

            entity.Property(e => e.FidocredentialId).HasColumnName("FIDOCredentialID");
            entity.Property(e => e.CredType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CredentialId)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("CredentialID");
            entity.Property(e => e.LastUsedDateTime).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PublicKey).IsRequired();
            entity.Property(e => e.RegDateTime).HasColumnType("datetime");
            entity.Property(e => e.UserHandle).IsRequired();
        });

        modelBuilder.Entity<FriendList>(entity =>
        {
            entity.HasKey(e => e.FriendListId).HasName("PK__FriendLi__64B949EBFF0464DA");

            entity.ToTable("FriendList");

            entity.Property(e => e.FriendListId).HasColumnName("FriendListID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FriendId).HasColumnName("FriendID");
            entity.Property(e => e.FriendStatus)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.InvitedMessage).HasMaxLength(50);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
        });

        modelBuilder.Entity<GenreList>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK_類型總表");

            entity.ToTable("GenreList");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<GenresForVideoList>(entity =>
        {
            entity.HasKey(e => e.SerialId);

            entity.ToTable("GenresForVideoList");

            entity.Property(e => e.SerialId).HasColumnName("SerialID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<GiftInfo>(entity =>
        {
            entity.HasKey(e => e.GiftId);

            entity.ToTable("GiftInfo");

            entity.Property(e => e.GiftId)
                .ValueGeneratedNever()
                .HasComment("贈品編號")
                .HasColumnName("GiftID");
            entity.Property(e => e.GiftDesc)
                .HasMaxLength(50)
                .HasComment("贈品說明");
            entity.Property(e => e.GiftName)
                .IsRequired()
                .HasMaxLength(10)
                .HasComment("贈品名稱");
            entity.Property(e => e.Pic).HasComment("贈品圖片");
            entity.Property(e => e.Qty).HasComment("贈品數量");
        });

        modelBuilder.Entity<GiftList>(entity =>
        {
            entity.HasKey(e => new { e.GiftListId, e.GiftId }).HasName("PK_GiftList_1");

            entity.ToTable("GiftList");

            entity.Property(e => e.GiftListId)
                .HasComment("贈品清單編號")
                .HasColumnName("GiftListID");
            entity.Property(e => e.GiftId)
                .HasComment("贈品編號")
                .HasColumnName("GiftID");
        });

        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(e => e.HallsId);

            entity.ToTable("Hall");

            entity.Property(e => e.HallsId)
                .ValueGeneratedNever()
                .HasColumnName("HallsID");
            entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
            entity.Property(e => e.HallsName).HasMaxLength(50);
        });

        modelBuilder.Entity<ImageForVideoList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ImageForWorkList");

            entity.ToTable("ImageForVideoList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<ImageList>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK_圖片列表");

            entity.ToTable("ImageList");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(300)
                .HasColumnName("imagePath");
        });

        modelBuilder.Entity<Invite>(entity =>
        {
            entity.HasKey(e => e.InvitelId);

            entity.ToTable("Invite");

            entity.Property(e => e.InvitelId)
                .HasComment("流水號")
                .HasColumnName("InvitelID");
            entity.Property(e => e.InviteMsg)
                .HasMaxLength(100)
                .HasComment("邀請訊息");
            entity.Property(e => e.InviteTime)
                .HasComment("邀請時間")
                .HasColumnType("datetime");
            entity.Property(e => e.InvitedMemberId)
                .HasComment("受邀好友ID")
                .HasColumnName("InvitedMemberID");
            entity.Property(e => e.MemberId)
                .HasComment("會員ID")
                .HasColumnName("MemberID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("狀態代碼:\r\nInitial初始(I)\r\nAccept接受(A)\r\nReject拒絕(R)\r\nPedding忽略(P)");
            entity.Property(e => e.StatusTime)
                .HasComment("狀態時間")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<KeywordForVideoList>(entity =>
        {
            entity.HasKey(e => e.SerialId).HasName("PK_KeywordFroVideoList");

            entity.ToTable("KeywordForVideoList");

            entity.Property(e => e.SerialId).HasColumnName("SerialID");
            entity.Property(e => e.KeywordId).HasColumnName("KeywordID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<KeywordList>(entity =>
        {
            entity.HasKey(e => e.KeywordId);

            entity.ToTable("KeywordList");

            entity.Property(e => e.KeywordId).HasColumnName("KeywordID");
            entity.Property(e => e.Keyword).HasMaxLength(50);
        });

        modelBuilder.Entity<MemberCoupon>(entity =>
        {
            entity.HasKey(e => e.SerialNo);

            entity.ToTable("MemberCoupon");

            entity.Property(e => e.SerialNo)
                .ValueGeneratedNever()
                .HasComment("優惠券序號(代碼)");
            entity.Property(e => e.ActionRefNo).HasComment("使用的交易單號");
            entity.Property(e => e.ActionType)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("使用類型(線上電影票/線下電影票)");
            entity.Property(e => e.CouponId)
                .HasComment("優惠券編號")
                .HasColumnName("CouponID");
            entity.Property(e => e.ExpireTime)
                .HasComment("失效時間")
                .HasColumnType("datetime");
            entity.Property(e => e.GetTime)
                .HasComment("得到優惠券時間")
                .HasColumnType("datetime");
            entity.Property(e => e.GiftId)
                .HasComment("兌換的贈品編號")
                .HasColumnName("GiftID");
            entity.Property(e => e.MemberId)
                .HasComment("會員編號")
                .HasColumnName("MemberID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("狀態(是否兌換,是否失效)");
            entity.Property(e => e.UseTime)
                .HasComment("使用時間")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MemberCreatedPlayList>(entity =>
        {
            entity.ToTable("MemberCreatedPlayList");

            entity.Property(e => e.MemberCreatedPlayListId).HasColumnName("MemberCreatedPlayListID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MemberInfo>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_MemberInformation");

            entity.ToTable("MemberInfo");

            entity.Property(e => e.MemberId)
                .ValueGeneratedNever()
                .HasComment("會員編號")
                .HasColumnName("MemberID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasComment("地址");
            entity.Property(e => e.Banned).HasDefaultValue(false);
            entity.Property(e => e.Birth).HasComment("生日");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("信箱");
            entity.Property(e => e.FidocredentialId).HasColumnName("FIDOCredentialID");
            entity.Property(e => e.Fidoenabled).HasColumnName("FIDOEnabled");
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("性別");
            entity.Property(e => e.Grade)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("會員等級");
            entity.Property(e => e.LastLoginDate)
                .HasComment("最後登入時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberIdentity).HasMaxLength(10);
            entity.Property(e => e.MemberName)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("會員姓名");
            entity.Property(e => e.NickName)
                .IsRequired()
                .HasMaxLength(10)
                .HasComment("暱稱");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("密碼");
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("手機");
            entity.Property(e => e.Point).HasComment("會員積分");
            entity.Property(e => e.RegisterDate).HasComment("註冊時間");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.UpdateTime)
                .HasComment("更新時間(會員到期,降級,升級由系統功能或者後台更改)")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(50)
                .HasComment("更新人員或系統功能");
        });

        modelBuilder.Entity<MemberNotice>(entity =>
        {
            entity.ToTable("MemberNotice");

            entity.Property(e => e.MemberNoticeId)
                .HasComment("會員通知編號")
                .HasColumnName("MemberNoticeID");
            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(10)
                .HasComment("建立通知的事件");
            entity.Property(e => e.CreTime)
                .HasComment("通知時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberId)
                .HasComment("會員編號")
                .HasColumnName("MemberID");
            entity.Property(e => e.NoticeContent)
                .IsRequired()
                .HasMaxLength(500)
                .HasComment("通知內容");
            entity.Property(e => e.RefNo).HasComment("通知參照的單號");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("狀態:已讀,未讀");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("通知標題");
        });

        modelBuilder.Entity<MemberPlayList>(entity =>
        {
            entity.HasKey(e => e.MemberPlayListId).HasName("PK__MemberPl__F763B3FA32E920B0");

            entity.ToTable("MemberPlayList");

            entity.Property(e => e.MemberPlayListId).HasColumnName("MemberPlayListID");
            entity.Property(e => e.AddedOtherMemberPlayListAt).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
        });

        modelBuilder.Entity<MemberRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.ToTable("MemberRating");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<NowShowingTheater>(entity =>
        {
            entity.HasKey(e => e.CombinationId);

            entity.ToTable("NowShowingTheater");

            entity.Property(e => e.CombinationId).HasColumnName("CombinationID");
            entity.Property(e => e.CinemaId).HasColumnName("CinemaID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.DeliveryAddress).HasMaxLength(50);
            entity.Property(e => e.DeliveryName).HasMaxLength(50);
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.LastEditTime).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderTotalPrice).HasColumnType("money");
            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<PlanList>(entity =>
        {
            entity.HasKey(e => e.PlanId);

            entity.ToTable("PlanList");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.PlanName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<PlayList>(entity =>
        {
            entity.HasKey(e => e.PlayListId).HasName("PK__PlayList__38709FBBFCBDB92F");

            entity.ToTable("PlayList");

            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.AnalysisTimestamp).HasColumnType("datetime");
            entity.Property(e => e.PlayListCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PlayListDescription).HasMaxLength(50);
            entity.Property(e => e.PlayListName).HasMaxLength(50);
            entity.Property(e => e.PlayListUpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<PlayListCollaborator>(entity =>
        {
            entity.HasKey(e => e.CollaboratorId).HasName("PK__PlayList__3AC201C3ED5C66AE");

            entity.ToTable("PlayListCollaborator");

            entity.Property(e => e.CollaboratorId).HasColumnName("CollaboratorID");
            entity.Property(e => e.ActionTimestamp).HasColumnType("datetime");
            entity.Property(e => e.CollaboratorActionType).HasMaxLength(50);
            entity.Property(e => e.CollaboratorJoinedAt).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
        });

        modelBuilder.Entity<PlayListItem>(entity =>
        {
            entity.HasKey(e => e.PlayListItemId).HasName("PK__PlayList__5B7B5B943F02F781");

            entity.ToTable("PlayListItem");

            entity.Property(e => e.PlayListItemId).HasColumnName("PlayListItemID");
            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.VideoAddedAt).HasColumnType("datetime");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<PointHistory>(entity =>
        {
            entity.ToTable("PointHistory");

            entity.Property(e => e.PointHistoryId)
                .ValueGeneratedNever()
                .HasComment("積分歷史編號")
                .HasColumnName("PointHistoryID");
            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("積分事件(發文,交易,邀請好友)");
            entity.Property(e => e.CreTime)
                .HasComment("事件時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasComment("會員編號")
                .HasColumnName("MemberID");
            entity.Property(e => e.Point).HasComment("積分");
            entity.Property(e => e.RefNo).HasComment("參照事件編號(發文,交易,邀請好友)");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).IsClustered(false);

            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.Lock).HasDefaultValue(true);
            entity.Property(e => e.PostContent).IsRequired();
            entity.Property(e => e.PostDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PosterId).HasColumnName("PosterID");
        });

        modelBuilder.Entity<PostUserReaction>(entity =>
        {
            entity.HasKey(e => e.CountId).HasName("PK__PostUserReac");
        });

        modelBuilder.Entity<ProductList>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("ProductList");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductDescription).HasMaxLength(100);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductPrice).HasColumnType("money");
        });

        modelBuilder.Entity<ReservationDetail>(entity =>
        {
            entity.HasKey(e => e.ReservationId);

            entity.ToTable("ReservationDetail");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TypeOfTicket)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<SeasonList>(entity =>
        {
            entity.HasKey(e => e.SeasonId);

            entity.ToTable("SeasonList");

            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.SeasonName).HasMaxLength(50);
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seat");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.HallsId).HasColumnName("HallsID");
            entity.Property(e => e.RowNumber).HasMaxLength(50);
            entity.Property(e => e.SeatStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<SeriesList>(entity =>
        {
            entity.HasKey(e => e.SeriesId).HasName("PK_系列總表");

            entity.ToTable("SeriesList");

            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
            entity.Property(e => e.SeriesName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<SessionSeat>(entity =>
        {
            entity.Property(e => e.SessionSeatId).HasColumnName("SessionSeatID");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.ToTable("ShoppingCart");

            entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        modelBuilder.Entity<ShowingHall>(entity =>
        {
            entity.ToTable("ShowingHall");

            entity.Property(e => e.ShowingHallId).HasColumnName("ShowingHallID");
            entity.Property(e => e.CombinationId).HasColumnName("CombinationID");
            entity.Property(e => e.HallsId).HasColumnName("HallsID");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.ToTable("Showtime");

            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
            entity.Property(e => e.HallsId).HasColumnName("HallsID");
            entity.Property(e => e.ShowTimeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("ShowTimeDATETIME");
            entity.Property(e => e.ViedoId).HasColumnName("ViedoID");
        });

        modelBuilder.Entity<StreamingPlatformList>(entity =>
        {
            entity.HasKey(e => e.PlatformId);

            entity.ToTable("StreamingPlatformList");

            entity.Property(e => e.PlatformId).HasColumnName("PlatformID");
            entity.Property(e => e.PlatformName).HasMaxLength(50);
        });

        modelBuilder.Entity<TheaterList>(entity =>
        {
            entity.HasKey(e => e.TheaterId);

            entity.ToTable("TheaterList");

            entity.Property(e => e.TheaterId)
                .ValueGeneratedNever()
                .HasColumnName("TheaterID");
            entity.Property(e => e.TheaterName).HasMaxLength(50);
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.ThemeId).HasName("PK__MovieDis__F9646BD21F408451");

            entity.ToTable("Theme");

            entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
            entity.Property(e => e.ThemeName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
        });

        modelBuilder.Entity<TypeList>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("TypeList");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TypeOfTicket>(entity =>
        {
            entity.HasKey(e => e.TypeOfTicket1);

            entity.ToTable("TypeOfTicket");

            entity.Property(e => e.TypeOfTicket1).HasColumnName("TypeOfTicket");
            entity.Property(e => e.TypeOfTicketName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserReaction>(entity =>
        {
            entity.HasKey(e => e.CountId).HasName("PK__UserReac");

            entity.HasIndex(e => new { e.MemberId, e.ArticleId }, "UQ_UserReactions_MemberArticle").IsUnique();

            entity.HasOne(d => d.Member).WithMany(p => p.UserReactions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReactions_MemberInfo");

            entity.HasOne(d => d.Member).WithMany(p => p.UserReactions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserReactions_MemberInfo");
        });

        modelBuilder.Entity<ValidCode>(entity =>
        {
            entity.ToTable("ValidCode");

            entity.Property(e => e.ValidCodeId)
                .ValueGeneratedNever()
                .HasComment("流水號")
                .HasColumnName("ValidCodeID");
            entity.Property(e => e.CreTime)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpireTime)
                .HasComment("到期時間")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasComment("會員編號")
                .HasColumnName("MemberID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("狀態");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(5)
                .HasComment("驗證類型(忘記密碼,註冊驗證)");
            entity.Property(e => e.ValidCode1)
                .IsRequired()
                .HasMaxLength(10)
                .HasComment("驗證碼")
                .HasColumnName("ValidCode");
        });

        modelBuilder.Entity<VideoList>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK_作品總表");

            entity.ToTable("VideoList");

            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.AgeRating).HasMaxLength(50);
            entity.Property(e => e.Bgpath)
                .HasMaxLength(300)
                .HasColumnName("BGPath");
            entity.Property(e => e.Lang).HasMaxLength(50);
            entity.Property(e => e.MainGenreId).HasColumnName("MainGenreID");
            entity.Property(e => e.Popularity).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.ThumbnailPath).HasMaxLength(300);
            entity.Property(e => e.TrailerUrl).HasColumnName("TrailerURL");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.VideoName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<VideoStreamingLinkList>(entity =>
        {
            entity.HasKey(e => e.StreamingId);

            entity.ToTable("VideoStreamingLinkList");

            entity.Property(e => e.StreamingId).HasColumnName("StreamingID");
            entity.Property(e => e.PlatformId).HasColumnName("PlatformID");
            entity.Property(e => e.StreamLink).HasMaxLength(100);
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}