USE [master]
GO
/****** Object:  Database [BSDataba]    Script Date: 5/15/2019 5:56:40 PM ******/
CREATE DATABASE [BSDataba]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BSDataba', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\BSDataba.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BSDataba_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\BSDataba_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BSDataba] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BSDataba].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BSDataba] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BSDataba] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BSDataba] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BSDataba] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BSDataba] SET ARITHABORT OFF 
GO
ALTER DATABASE [BSDataba] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BSDataba] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [BSDataba] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BSDataba] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BSDataba] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BSDataba] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BSDataba] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BSDataba] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BSDataba] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BSDataba] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BSDataba] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BSDataba] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BSDataba] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BSDataba] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BSDataba] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BSDataba] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BSDataba] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BSDataba] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BSDataba] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BSDataba] SET  MULTI_USER 
GO
ALTER DATABASE [BSDataba] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BSDataba] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BSDataba] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BSDataba] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [BSDataba]
GO
/****** Object:  User [sas]    Script Date: 5/15/2019 5:56:40 PM ******/
CREATE USER [sas] FOR LOGIN [sas] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  StoredProcedure [dbo].[ShowAllBook]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ShowAllBook]
	@sortID int
as
begin
	--liệt kê theo sách xem nhiều sortID = 1
	if(@sortID = 1)
		begin
			select * from Books 
			order by ViewCount desc;
		end
	-- liệt kê sách mới theo publicationDate
	else if(@sortID = 2)
		begin
			select * from Books 
			order by PublicationDate desc;
		end
	-- liệt kê sách hot (sách được đặt nhiều)
	else if(@sortID = 3)
		begin
			--SELECT b.BookID,b.Name,b.ImageBoolID,b.Price, b.Overview, b.PublisherID, b.PublicationDate,
			--b.Overview,b.Details,b.TotalQuantity, b.ViewCount, b.isDeleted
			--FROM Books b 
			--GROUP BY b.BookID,b.Name,b.ImageBoolID,b.Price, b.Overview, b.PublisherID,b.PublicationDate,
			--b.Overview,b.Details,b.TotalQuantity, b.ViewCount, b.isDeleted
			SELECT * from Books
			order by Price desc;
		end
	else if(@sortID = 4)
		begin
			SELECT * from Books
			order by Price asc;
		end
	-- liệt kê sách bình thường thôi ko sắp xếp gì hết
	else if(@sortID = 0)
		begin
			select * from Books; 
		end
end







GO
/****** Object:  StoredProcedure [dbo].[ShowBookByPublisherCategoryAndSortID]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ShowBookByPublisherCategoryAndSortID]
	@categoryID int,
	@publicsherID int,
	@sortID int
as
begin
	--liệt kê theo sách xem nhiều sortID = 1
	if(@sortID = 1)
		begin
			select * from Books 
			where (PublisherID = @publicsherID) AND (BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID))
			order by ViewCount desc;
		end
	-- liệt kê sách mới theo publicationDate
	else if(@sortID = 2)
		begin
			select * from Books 
			where (PublisherID = @publicsherID) AND (BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)) 
			order by PublicationDate desc;
		end
	-- liệt kê sách theo giá giảm dần
	else if(@sortID = 3)
		begin
			select * from Books 
			where (PublisherID = @publicsherID) AND (BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)) 
			order by Price desc;
		end
	else if(@sortID = 4)
		begin
			select * from Books 
			where (PublisherID = @publicsherID) AND (BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)) 
			order by Price asc;
		end
	-- liệt kê sách bình thường thôi ko sắp xếp gì hết
	else if(@sortID = 0)
		begin
			select * from Books
			where (PublisherID = @publicsherID) AND (BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID))
			order by BookID; 
		end
end







GO
/****** Object:  StoredProcedure [dbo].[ShowBookFollowCategory]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ShowBookFollowCategory]
	@categoryID int,
	@sortID int
as
-- condition = 1 sắp xếp theo thể loại
begin
	--liệt kê theo sách xem nhiều sortID = 1
	if(@sortID = 1)
		begin
			select * from Books 
			where BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID) 
			order by ViewCount desc;
		end
	-- liệt kê sách mới theo publicationDate
	else if(@sortID = 2)
		begin
			select * from Books 
			where BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID) 
			order by PublicationDate desc;
		end
	-- liệt kê sách hot (sách được đặt nhiều)
	-- liệt kê sách giảm dần theo giá
	else if(@sortID = 3)
		begin
			--SELECT b.BookID,b.Name,b.ImageBoolID,b.Price, b.Overview, b.PublisherID, b.PublicationDate,
			--b.Overview,b.Details,b.TotalQuantity, b.ViewCount, COUNT(r.BookID) as temp
			--FROM Books b RIGHT JOIN
			--OrdersDetails r ON b.BookID = r.BookID
			--where b.BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)
			--GROUP BY b.BookID,b.Name,b.ImageBoolID,b.Price, b.Overview, b.PublisherID,b.PublicationDate,
			--b.Overview,b.Details,b.TotalQuantity, b.ViewCount
			--order by temp desc
			SELECT * from Books
			where BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)
			order by Price desc;
		end
	-- liệt kê sách tăng dần theo giá
	else if(@sortID = 4)
		begin
			SELECT * from Books
			where BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID)
			order by Price asc;
		end
	-- liệt kê sách bình thường thôi ko sắp xếp gì hết
	else if(@sortID = 0)
		begin
			select * from Books 
			where BookID in (select BookID from CategoriesBooks where CategoryID = @categoryID);
		end
end





GO
/****** Object:  StoredProcedure [dbo].[ShowBookFollowKeySearch]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ShowBookFollowKeySearch]
	@keySearch nvarchar(MAX)
as
begin
	select * 
	from Books b
	where [dbo].[funChuyenCoDauThanhKhongDau](b.Name) like '%'+@keySearch+'%'
end







GO
/****** Object:  StoredProcedure [dbo].[ShowBookFollowPublisher]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[ShowBookFollowPublisher]
	@publicsherID int,
	@sortID int
as
begin
	--liệt kê theo sách xem nhiều sortID = 1
	if(@sortID = 1)
		begin
			select * from Books 
			where PublisherID = @publicsherID
			order by ViewCount desc;
		end
	-- liệt kê sách mới theo publicationDate
	else if(@sortID = 2)
		begin
			select * from Books 
			where PublisherID = @publicsherID 
			order by PublicationDate desc;
		end
	-- liệt kê sách hot (sách được đặt nhiều)
	else if(@sortID = 3)
		begin
			select * from Books 
			where PublisherID = @publicsherID 
			order by Price desc;
		end
	else if(@sortID = 4)
		begin
			select * from Books 
			where PublisherID = @publicsherID 
			order by Price asc;
		end
	-- liệt kê sách bình thường thôi ko sắp xếp gì hết
	else if(@sortID = 0)
		begin
			select * from Books 
			where PublisherID = @publicsherID;
		end
end







GO
/****** Object:  StoredProcedure [dbo].[spGetBookName]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[spGetBookName]
@Name nvarchar(MAX)
as
Begin
	select Name
	from dbo.Books
	where Name like + '%'
end







GO
/****** Object:  StoredProcedure [dbo].[UPDATEAUTHORIMG]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[UPDATEAUTHORIMG]
	@imageURL nvarchar(MAX),
	@authorID int
as
begin
	update Images
	set ImageURL = @imageURL
	where ImageBoolID = (select img.ImageBoolID from ImageBool img, Authors au where img.ImageBoolID = au.ImageBoolID and  AuthorID = @authorID);
end








GO
/****** Object:  UserDefinedFunction [dbo].[fuChuyenCoDauThanhKhongDau]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fuChuyenCoDauThanhKhongDau]
(
      @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN    
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)

    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
                  ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
                  ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
                  ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
                  +NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
                  iiiiiooooooooooooooouuuuuuuuuuyyyyy
                  AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
                  OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'

    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1

    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN  
      SET @COUNTER1 = 1
      --Tìm trong chuỗi mẫu
       WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
       BEGIN
	   -- unicode trả về kiểu int
     IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) -- lấy ra 1 kí tự unicode để so sánh với chuỗi input vào
            = UNICODE(SUBSTRING(@strInput,@COUNTER ,1)) -- lấy ra 1 kí tự input vào
     BEGIN          
          IF @COUNTER=1
		  -- bằng 1 thì chỉ việc chuyển chữ đầu tiên thành kí tự ko unicode, sau đó cộng thêm chuỗi từ ký tự đó trở đi
              SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) -- lấy ra ký tự ko unicode của chuỗi nhập vào
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) -- lấy ra chuỗi từ ký tự đã bỏ unicode trở đi                  
          ELSE
              SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) -- lấy ra chuỗi trước ký tự unicode 
              +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) -- lấy ra ký đã bỏ unicode
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) -- lấy ra chuỗi sau ký tự unicode
              BREAK
               END
             SET @COUNTER1 = @COUNTER1 + 1
       END
      --Tìm tiếp
       SET @COUNTER = @COUNTER +1
    END

    RETURN @strInput
END







GO
/****** Object:  UserDefinedFunction [dbo].[funChuyenCoDauThanhKhongDau]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[funChuyenCoDauThanhKhongDau]
(
      @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN    
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)

    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
                  ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
                  ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
                  ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
                  +NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
                  iiiiiooooooooooooooouuuuuuuuuuyyyyy
                  AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
                  OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'

    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1

    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN  
      SET @COUNTER1 = 1
      --Tìm trong chuỗi mẫu
       WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
       BEGIN
	   -- unicode trả về kiểu int
     IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) -- lấy ra 1 kí tự unicode để so sánh với chuỗi input vào
            = UNICODE(SUBSTRING(@strInput,@COUNTER ,1)) -- lấy ra 1 kí tự input vào
     BEGIN          
          IF @COUNTER=1
		  -- bằng 1 thì chỉ việc chuyển chữ đầu tiên thành kí tự ko unicode, sau đó cộng thêm chuỗi từ ký tự đó trở đi
              SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) -- lấy ra ký tự ko unicode của chuỗi nhập vào
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) -- lấy ra chuỗi từ ký tự đã bỏ unicode trở đi                  
          ELSE
              SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) -- lấy ra chuỗi trước ký tự unicode 
              +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) -- lấy ra ký đã bỏ unicode
              + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) -- lấy ra chuỗi sau ký tự unicode
              BREAK
               END
             SET @COUNTER1 = @COUNTER1 + 1
       END
      --Tìm tiếp
       SET @COUNTER = @COUNTER +1
    END

    RETURN @strInput
END





GO
/****** Object:  Table [dbo].[Authors]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[AuthorID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PenName] [nvarchar](100) NULL,
	[Overview] [nvarchar](max) NULL,
	[ImageBoolID] [int] NULL,
	[isDeleted] [bit] NULL,
 CONSTRAINT [PK_dbo.Authors] PRIMARY KEY CLUSTERED 
(
	[AuthorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AuthorsBooks]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthorsBooks](
	[AuthorID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AuthorsBooks] PRIMARY KEY CLUSTERED 
(
	[AuthorID] ASC,
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Books]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[PublisherID] [int] NULL,
	[PublicationDate] [date] NULL,
	[ImageBoolID] [int] NULL,
	[Overview] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Price] [float] NULL,
	[TotalQuantity] [int] NOT NULL,
	[ViewCount] [int] NOT NULL,
	[isDeleted] [bit] NULL,
	[CurrentQuantity] [int] NULL,
 CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[isDeleted] [bit] NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoriesBooks]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriesBooks](
	[BookID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CategoriesBooks] PRIMARY KEY CLUSTERED 
(
	[BookID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[isDeleted] [bit] NULL,
	[isLike] [int] NULL,
	[FolderID] [int] NULL,
 CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FilterPrice]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FilterPrice](
	[FilterID] [int] NOT NULL,
	[PriceFrom] [float] NULL,
	[PriceTo] [float] NULL,
	[BookID] [int] NOT NULL,
 CONSTRAINT [PK_FilterPrice] PRIMARY KEY CLUSTERED 
(
	[FilterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[historyBankChargings]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[historyBankChargings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[fullname] [nvarchar](max) NULL,
	[phone] [nvarchar](max) NULL,
	[email] [nvarchar](max) NULL,
	[transaction_info] [nvarchar](max) NULL,
	[order_code] [nvarchar](max) NULL,
	[price] [int] NULL,
	[payment_id] [nvarchar](max) NULL,
	[payment_type] [nvarchar](max) NULL,
	[error_text] [nvarchar](max) NULL,
	[secure_code] [nvarchar](max) NULL,
	[date_trans] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.historyBankChargings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImageBool]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageBool](
	[ImageBoolID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_dbo.ImageBool] PRIMARY KEY CLUSTERED 
(
	[ImageBoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[ImageBoolID] [int] NOT NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Images] PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrdersBooks]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersBooks](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[FoundedDate] [datetime] NULL,
	[UserID] [int] NOT NULL,
	[Status] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[Phone] [nchar](50) NULL,
	[Email] [nchar](50) NULL,
	[FullName] [nvarchar](max) NULL,
	[Paid] [bit] NULL,
	[Canceled] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.OrdersBooks] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrdersDetails]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersDetails](
	[OrderID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[Total] [float] NULL,
	[Quantity] [int] NULL,
	[CreatedDate] [date] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_dbo.OrdersDetails] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publishers](
	[PublisherID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](20) NULL,
	[Note] [nvarchar](max) NULL,
	[ImageURL] [nvarchar](max) NULL,
	[isDeleted] [bit] NULL,
 CONSTRAINT [PK_dbo.Publishers] PRIMARY KEY CLUSTERED 
(
	[PublisherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserRoleID] [int] NOT NULL,
	[Title] [nchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/15/2019 5:56:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Birthday] [date] NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[ImageURL] [nvarchar](max) NULL,
	[UserRoleID] [int] NOT NULL,
	[isActivated] [bit] NOT NULL,
	[Address] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (1, N'Yuriko Mamiya', N'Yuriko Mamiya', N'Late Autumn follows the attempts of three older men to help the widow of a late friend to marry off her daughter. The daughter is less than happy at the proposals, mainly because of her reluctance to leave her mother alone. The film was selected as the Japanese entry for the Best Foreign Language Film at the 33rd Academy Awards, but was not accepted as a nominee.[1] While not one of the works for which Ozu is most known, Late Autumn is highly regarded by critics.', 1, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (2, N'Paulo Coelho', N'Paulo Coelho', N'<div>
	Paulo Coelho (sinh ngày 24 tháng 8 năm 1947; phát âm như "Pao-lu Koe-lhu") là tiểu thuyết gia nổi tiếng người Brasil.</div>
<div>
	Sách của ông đã được bán ra hơn 86 triệu bản trên 150 nước và được dịch ra 56 thứ tiếng. Ông đã nhận được nhiều giải thưởng của nhiều nước, trong đó tác phẩm Veronika quyết chết (Veronika decide morrer) được đề cử cho Giải Văn chương Dublin IMPAC Quốc tế có uy tín.</div>
', 2, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (3, N'Shinkai Makoto', N'Shinkai Makoto', N'<div>
	Shinkai Makoto (新海誠 Tân Hải Thành?), tên khai sinh là Nītsu Makoto (新津誠 Tân Tân Thành?) là một nhà làm phim, đạo diễn, trước kia là nhà thiết kế đồ hoạ xuất thân từ quận Minamisaku, Nagano, Nhật Bản. Ông được biết đến khi đạo diễn bộ phim anime có doanh thu toàn cầu cao nhất mọi thời đại Your Name – Tên cậu là gì?.</div>
<div>
	Nhiều trang như Anime Advocates hay ActiveAnime đã so sánh ông như là "Miyazaki mới" ("New Miyazaki"), nhưng ông gọi đó là "đánh giá quá cao".[1] Tên ông được đặt cho tiểu hành tinh 55222 Makotoshinkai.</div>
', 3, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (4, N'Robin Sharma', N'Robin Sharma', N'Robin Sharma là một trong những nhà cố vấn được tin cậy nhất thế giới trong việc lãnh đạo cá nhân và tổ chức. Là người sáng lập Tập đoàn Sharma Leadership International, một công ty tư vấn chuyên giúp các tổ chức phát triển những nhân viên Lãnh-đạo-Không-Chức-danh, các khách hàng của ông bao gồm nhiều công ty trong danh sách Fortune 500 như Microsoít, IBM, GE, FedEx, BP, Nike, Unilever và Kraft, cũng như những tổ chức như Đại học Yale và YPO. Trong một cuộc khảo sát độc lập về những nhà cố vấn lãnh đạo hàng đầu, Robin Sharma xếp thứ 2, cùng với Jack Welch và Rudy Giuliani.', 4, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (5, N'Dale Carnegie', N'Dale Carnegie', N'Dale Breckenridge Carnegie (trước kia là Carnagey cho tới năm 1922 và có thể một thời gian muộn hơn) (24 tháng 11 năm 1888 – 1 tháng 11 năm 1955) là một nhà văn và nhà thuyết trình Mỹ và là người phát triển các lớp tự giáo dục, nghệ thuật bán hàng, huấn luyện đoàn thể, nói trước công chúng và các kỹ năng giao tiếp giữa mọi người. Ra đời trong cảnh nghèo đói tại một trang trại ở Missouri, ông là tác giả cuốn Đắc Nhân Tâm, được xuất bản lần đầu năm 1936, một cuốn sách thuộc hàng bán chạy nhất và được biết đến nhiều nhất cho đến tận ngày nay. Ông cũng viết một cuốn tiểu sử Abraham Lincoln, với tựa đề Lincoln con người chưa biết, và nhiều cuốn sách khác.', 5, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (6, N'Rosie Nguyễn', N'Rosie Nguyễn', N'Rosie Nguyễn tên thật là Nguyễn Hoàng Nguyên, một tác giả sách, blogger/facebooker về văn hóa du lịch, giảng viên lớp học kỹ năng, và huấn luyện viên yoga. Ngoài việc viết lách và giảng dạy thì là một người tự học, một ta ba lô, một kẻ mộng mơ và tràn đầy tình yêu cuộc sống.', 6, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (7, N'Adam Khoo', N'Adam Khoo', N'Adam Khoo cũng là một trong những triệu phú trẻ giàu có nhất Singapore, tác giả của nhiều quyển sách bán chạy nhất, doanh nhân và diễn giả hàng đầu Châu Á.', 7, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (8, N'Lâu Vũ Tình', N'Lâu Vũ Tình', N'Một nhà văn ngôn tình cho giới trẻ đang được yêu thích.', 8, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (9, N'Nguyễn Nhật Ánh', N'Nguyễn Nhật Ánh', N'<div>
	Nguyễn Nhật Ánh là tên và cũng là bút danh của một nhà văn Việt Nam chuyên viết cho tuổi mới lớn.</div>
<div>
	Thuở nhỏ ông theo học tại các trường Tiểu La, Trần Cao Vân và Phan Chu Trinh. Từ 1973 Nguyễn Nhật Ánh chuyển vào sống tại Sài Gòn, theo học ngành sư phạm. Ông đã từng đi Thanh niên xung phong, dạy học, làm công tác Đoàn Thanh niên Cộng Sản Hồ Chí Minh. Từ 1986 đến nay ông là phóng viên nhật báo Sài Gòn Giải Phóng, lần lượt viết về sân khấu, phụ trách mục tiểu phẩm, phụ trách trang thiếu nhi và hiện nay là bình luận viên thể thao trên báo Sài Gòn Giải Phóng Chủ nhật với bút danh Chu Đình Ngạn. Ngoài ra, Nguyễn Nhật Ánh còn có những bút danh khác như Anh Bồ Câu, Lê Duy Cật, Đông Phương Sóc, Sóc Phương Đông,...</div>
', 9, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (10, N'Tony Buổi Sáng', N'Tony Buổi Sáng', N'Cho đến nay, mặc dù đã có rất nhiều bài viết xung quanh hai cuốn sách bestseller&nbsp; “Cà phê cùng Tony” và “Trên đường băng” nhưng tác giả vẫn là một ẩn số. Cư dân mạng tò mò và có đưa ra một vài hình ảnh cho là của Tony nhưng đấy đều là những hình ảnh “chưa có kiểm chứng”. Còn báo chí chính thống thì chưa thấy xuất hiện chân dung của Tony.', 10, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (11, N'Takeshi Furukawa', N'Takeshi Furukawa', N'Takeshi Furukawa là nhà soạn nhạc kiêm nhạc trưởng người Mỹ gốc Nhật được chỉ định bởi BAFTA. Tác phẩm của anh đã mở rộng giai đoạn hòa nhạc, phim, truyền hình, trò chơi điện tử và chiến dịch quảng cáo.', 11, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (12, N'Baird T. Spalding', N'Baird T. Spalding', N'Baird Thomas Spalding là một nhà văn tinh thần người Mỹ, tác giả của loạt sách thiêng liêng: Đời sống và giảng dạy của các bậc thầy của Viễn Đông.', 12, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (13, N'Minh Niệm', N'Minh Niệm', N'Sinh tại Châu Thành, Tiền Giang, xuất gia tại Phật Học Viện Huệ Nghiêm – Sài Gòn, Minh Niệm từng thọ giáo thiền sư Thích Nhất Hạnh tại Pháp và thiền sư Tejaniya tại Mỹ. Kết quả sau quá trình tu tập, lĩnh hội kiến thức… Ông quyết định chọn con đường hướng dẫn thiền và khai triển tâm lý trị liệu cho giới trẻ làm Phật sự của mình. Tiếp cận với nhiều người trẻ, lắng nghe thế giới quan của họ và quan sát những đổi thay trong đời sống hiện đại, ông phát hiện có rất nhiều vấn đề của cuộc sống.', 13, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (14, N'TS. David J. Lieberman', N'David J. Lieberman', N'<div>
	Sinh: 1953. Tiến sĩ David J. Lieberman là một tác giả được trao tặng nhiều giải thưởng, và được quốc tế công nhận là chuyên gia hàng đầu trong lĩnh vực nghiên cứu về hành vi và các mối quan hệ của con người.</div>
<div>
	Ông đã xuất bản 6 cuốn sách, tất cả đều được dịch ra 18 thứ tiếng và hai cuốn lọt vào danh sách những cuốn sách bán chạy nhất của tờ New York Times. Ông còn là khách mời của hơn 200 chương trình truyền hình như The Today Show, Fox News, PBS và The View. Ngoài ra, ông giảng dạy và tổ chức các cuộc hội thảo về nhiều lĩnh vực trên toàn nước Mỹ.</div>
', 14, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (15, N'Nelle Harper Lee', N'Harper Lee', N'<div>
	Nelle Harper Lee (28 tháng 4 năm 1926 – 19 tháng 2 năm 2016), thường được biết tới với tên Harper Lee, là một nữ nhà văn người Mỹ. Bà được biết tới nhiều nhất qua tiểu thuyết đầu tay Giết con chim nhại (To Kill a Mockingbird). Ngày 5 tháng 11 năm 2007, Harper Lee đã được tổng thống George W. Bush trao Huân chương Tự do Tổng thống Hoa Kỳ (Presidential Medal of Freedom), huân chương cao quý nhất dành cho công dân Hoa Kỳ, vì những đóng góp của bà cho văn học Mỹ.</div>
<div>
	Vào tháng 2 năm 2015, luật sư của Lee xác nhận xuất bản cuốn tiểu thuyết thứ 2, Hãy đi đặt người canh gác (Go Set a Watchman). Được sáng tác vào giữa thập niên 1950, quyển sách phát hành vào tháng 7 năm 2015 như là phần tiếp theo của Giết con chim nhại.</div>
', 15, 0)
INSERT [dbo].[Authors] ([AuthorID], [Name], [PenName], [Overview], [ImageBoolID], [isDeleted]) VALUES (16, N'David J. Schwartz. Ph.D', N'David J. Schwartz.', N'<div>
	David Joseph Schwartz (23 tháng 3 năm 1927 – 6 tháng 12 năm 1987)[1] là nhà văn, giáo sư Đại học bang Georgia, được biết đến sau tác phẩm Sự kỳ diệu của tư duy lớn năm 1959.</div>
<div>
	Ngoài việc giảng dạy, ông còn là chủ tịch của Creative Educational Services, một công ty chuyên tư vấn phát triển kỹ năng lãnh đạo. Ông được xem là một trong những chuyên gia hàng đầu thế giới trong lĩnh vực phát triển tiềm năng con người. Công ty của ông đã cung cấp dịch vụ tư vấn trực tiếp, thuyết trình và hội thảo cho hàng nghìn khách hàng là lãnh đạo của các công ty, tập đoàn tại Mỹ và các quốc gia trên toàn thế giới.</div>
', 16, 0)
SET IDENTITY_INSERT [dbo].[Authors] OFF
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (1, 1)
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (2, 2)
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (3, 3)
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (4, 4)
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (5, 79)
INSERT [dbo].[AuthorsBooks] ([AuthorID], [BookID]) VALUES (16, 78)
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (1, N'Bốn Chàng Trai Cùng Nhau Đi Du Lịch', 1, CAST(0xF13D0B00 AS Date), 17, N'Bốn Chàng Trai Cùng Nhau Đi Du Lịch', N'Mashima – một anh chàng thật thà và thụ động. Shigeta – một nhà nghiên cứu thường thường bậc trung, đã có một đời vợ. Nakasugi – một anh chàng luôn bị cô bạn gái kiểm soát, có ưu điểm là dễ gây thiện cảm cho người đối diện. Và Saiki – Một anh chàng cực kì đẹp trai nhưng tính khí lạ đời. Không phải bạn bè cũng chẳng đặc biệt thân thích, vậy mà bốn con người ấy lại đồng hành cùng nhau trên những chuyến đi. Qua mỗi chuyến đi họ trở nên thân thiết hơn nhưng vẫn luôn cố gắng giữ một khoảng cách, không can thiệp quá sâu vào sự riêng tư của những người bạn đồng hành nhưng vẫn luôn sẵn sàng đưa tay giúp đỡ, an ủi lẫn nhau. Cuốn sách để lại cho người đọc một cảm giác vô cùng dễ chịu.', 71000, 74, 0, NULL, 71)
INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (2, N'Nhà Giả Kim', 2, CAST(0xA5370B00 AS Date), 18, N'Nhà Giả Kim', N'Tất cả những trải nghiệm trong chuyến phiêu du theo đuổi vận mệnh của mình đã giúp Santiago thấu hiểu được ý nghĩa sâu xa nhất của hạnh phúc, hòa hợp với vũ trụ và con người.

	Tiểu thuyết Nhà giả kim của Paulo Coelho như một câu chuyện cổ tích giản dị, nhân ái, giàu chất thơ, thấm đẫm những minh triết huyền bí của phương Đông. Trong lần xuất bản đầu tiên tại Brazil vào năm 1988, sách chỉ bán được 900 bản. Nhưng, với số phận đặc biệt của cuốn sách dành cho toàn nhân loại, vượt ra ngoài biên giới quốc gia, Nhà giả kim đã làm rung động hàng triệu tâm hồn, trở thành một trong những cuốn sách bán chạy nhất mọi thời đại, và có thể làm thay đổi cuộc đời người đọc.

	“Nhưng nhà luyện kim đan không quan tâm mấy đến những điều ấy. Ông đã từng thấy nhiều người đến rồi đi, trong khi ốc đảo và sa mạc vẫn là ốc đảo và sa mạc. Ông đã thấy vua chúa và kẻ ăn xin đi qua biển cát này, cái biển cát thường xuyên thay hình đổi dạng vì gió thổi nhưng vẫn mãi mãi là biển cát mà ông đã biết từ thuở nhỏ. Tuy vậy, tự đáy lòng mình, ông không thể không cảm thấy vui trước hạnh phúc của mỗi người lữ khách, sau bao ngày chỉ có cát vàng với trời xanh nay được thấy chà là xanh tươi hiện ra trước mắt. ‘Có thể Thượng đế tạo ra sa mạc chỉ để cho con người biết quý trọng cây chà là,’ ông nghĩ.”', 41000, 10, 2, 0, 6)
INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (3, N'5 Centimet Trên Giây', 2, CAST(0x4F390B00 AS Date), 19, N'5 Centimet Trên Giây', N'5cm/s không chỉ là vận tốc của những cánh anh đào rơi, mà còn là vận tốc khi chúng ta lặng lẽ bước qua đời nhau, đánh mất bao cảm xúc thiết tha nhất của tình yêu.

	Bằng giọng văn tinh tế, truyền cảm, 5 centimet trên giây mang đến những khắc họa mới về tâm hồn và khả năng tồn tại của cảm xúc, bắt đầu từ tình yêu trong sáng, ngọt ngào của một cô bé và cậu bé. Ba giai đoạn, ba mảnh ghép, ba ngôi kể chuyện khác nhau nhưng đều xoay quanh nhân vật nam chính, người luôn bị ám ảnh bởi kí ức và những điều đã qua…

	Khác với những câu chuyện cuốn bạn chạy một mạch, truyện này khó mà đọc nhanh. Ngón tay bạn hẳn sẽ ngừng lại cả trăm lần trên mỗi trang sách. Chỉ vì một cử động rất khẽ, một câu thoại, hay một xúc cảm bất chợt có thể sẽ đánh thức những điều tưởng chừng đã ngủ quên trong tiềm thức, như ngọn đèn vừa được bật sáng trong tâm trí bạn. Và rồi có lúc nó vượt quá giới hạn chịu đựng, bạn quyết định gấp cuốn sách lại chỉ để tận hưởng chút ánh sáng từ ngọn đèn, hay đơn giản là để vết thương trong lòng mình có thời gian tự tìm xoa dịu.', 30000, 10, 6, 0, 6)
INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (4, N'Đời Ngắn Đừng Ngủ Dài', 3, CAST(0x79380B00 AS Date), 20, N'Đời Ngắn Đừng Ngủ Dài', N'“Mọi lựa chọn đều giá trị. Mọi bước đi đều quan trọng. Cuộc sống vẫn diễn ra theo cách của nó, không phải theo cách của ta. Hãy kiên nhẫn. Tin tưởng. Hãy giống như người thợ cắt đá, đều đặn từng nhịp, ngày qua ngày. Cuối cùng, một nhát cắt duy nhất sẽ phá vỡ tảng đá và lộ ra viên kim cương. Người tràn đầy nhiệt huyết và tận tâm với việc mình làm không bao giờ bị chối bỏ. Sự thật là thế.”

	Bằng những lời chia sẻ thật ngắn gọn, dễ hiểu về những trải nghiệm và suy ngẫm trong đời, Robin Sharma tiếp tục phong cách viết của ông từ cuốn sách Điều vĩ đại đời thường để mang đến cho độc giả những bài viết như lời tâm sự, vừa chân thành vừa sâu sắc.', 36000, 12, 12, 0, 9)
INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (78, N'Anohana', 3, CAST(0xAA3F0B00 AS Date), 88, N'định dạng 1', N'định dạng 2 ', 20000, 1, 1, 0, 0)
INSERT [dbo].[Books] ([BookID], [Name], [PublisherID], [PublicationDate], [ImageBoolID], [Overview], [Details], [Price], [TotalQuantity], [ViewCount], [isDeleted], [CurrentQuantity]) VALUES (79, N'Sakamoto', 2, CAST(0xA93F0B00 AS Date), 89, N'định dạng 1', N'định dạng 2 ', 20000, 12, 1, 0, 12)
SET IDENTITY_INSERT [dbo].[Books] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (1, N'Công Nghệ Thông Tin', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (2, N'Du Lịch', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (3, N'Khoa Học', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (4, N'Kinh Tế', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (5, N'Mỹ Thuật', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (6, N'Ngoại Ngữ', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (7, N'Sách Giáo Khoa', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (8, N'Văn hóa - Xã hội', 0)
INSERT [dbo].[Categories] ([CategoryID], [Name], [isDeleted]) VALUES (9, N'Văn Học Nước Ngoài', 0)
SET IDENTITY_INSERT [dbo].[Categories] OFF
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (1, 2)
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (2, 9)
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (3, 8)
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (4, 1)
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (78, 9)
INSERT [dbo].[CategoriesBooks] ([BookID], [CategoryID]) VALUES (79, 9)
SET IDENTITY_INSERT [dbo].[ImageBool] ON 

INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (1)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (2)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (3)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (4)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (5)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (6)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (7)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (8)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (9)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (10)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (11)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (12)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (13)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (14)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (15)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (16)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (17)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (18)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (19)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (20)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (21)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (22)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (23)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (24)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (25)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (26)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (27)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (28)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (29)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (30)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (31)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (32)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (33)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (34)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (35)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (36)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (37)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (38)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (39)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (40)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (41)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (42)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (43)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (44)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (45)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (46)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (47)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (48)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (49)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (50)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (51)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (52)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (53)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (54)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (55)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (56)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (57)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (58)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (59)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (60)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (61)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (62)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (63)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (64)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (65)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (66)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (67)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (68)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (69)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (70)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (71)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (72)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (73)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (74)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (75)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (76)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (77)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (78)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (79)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (80)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (81)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (82)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (83)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (84)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (85)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (86)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (87)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (88)
INSERT [dbo].[ImageBool] ([ImageBoolID]) VALUES (89)
SET IDENTITY_INSERT [dbo].[ImageBool] OFF
SET IDENTITY_INSERT [dbo].[Images] ON 

INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (1, 1, N'/Content/files/Yuriko%20Mamiya.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (2, 2, N'/Content/files/Paulo%20Coelho.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (3, 3, N'/Content/files/Shinkai%20Makoto.JPG')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (4, 4, N'/Content/files/Robin%20Sharma.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (5, 5, N'/Content/files/Dale%20Carnegie.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (6, 6, N'/Content/files/Rosie%20Nguy%E1%BB%85n.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (7, 7, N'/Content/files/Adam%20Khoo.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (8, 8, N'/Content/files/L%C3%A2u%20V%C5%A9%20T%C3%ACnh.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (9, 9, N'/Content/files/Nguyen_Nhat_Anh.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (10, 10, N'/Content/files/Tony-Bu%E1%BB%95i-S%C3%A1ng.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (11, 11, N'/Content/files/Takeshi%20Furukawa.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (12, 12, N'/Content/files/Baird%20T_%20Spalding.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (13, 13, N'/Content/files/Minh%20Ni%E1%BB%87m.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (14, 14, N'/Content/files/ts_david_j_lieberman.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (15, 15, N'/Content/files/elle%20Harper%20Lee.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (16, 16, N'/Content/files/David-J-Schwartz.jpeg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (17, 17, N'/Content/files/31faa41dfe6feae3b7303bbb1e36db27.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (18, 18, N'/Content/files/img117.u3059.d20170616.t100547.729023.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (19, 19, N'/Content/files/5-cm-tr_n-giay.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (20, 20, N'/Content/files/doi-ngan-dung-ngu-dai.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (21, 21, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (22, 22, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (23, 23, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (24, 24, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (25, 25, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (26, 26, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (27, 27, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (28, 28, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (29, 29, N'/Content/files/3de86b8c89986bc63289.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (30, 30, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (31, 31, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (32, 32, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (33, 33, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (34, 34, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (35, 35, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (36, 36, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (37, 37, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (38, 38, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (39, 39, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (40, 40, N'1')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (41, 41, N'/Content/files/3de86b8c89986bc63289.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (42, 42, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (43, 43, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (44, 44, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (45, 46, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (46, 47, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (47, 48, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (49, 50, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (50, 51, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (51, 52, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (52, 53, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (53, 54, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (54, 55, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (55, 56, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (56, 57, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (57, 58, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (58, 59, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (59, 60, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (60, 61, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (61, 62, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (62, 63, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (63, 64, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (64, 65, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (65, 66, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (66, 67, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (67, 68, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (68, 69, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (69, 70, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (71, 76, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (72, 78, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (73, 79, N'/Content/files/3de86b8c89986bc63289.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (74, 80, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (75, 81, N'/Content/files/images.png')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (76, 82, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (77, 83, N'/Content/files/toi-di-hoc-700x1000.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (78, 84, N'/Content/files/3de86b8c89986bc63289.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (79, 85, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (80, 86, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (81, 87, N'/Content/files/HayChamSocMe.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (82, 88, N'/Content/files/220px-AnoHana_DVD_vol_1.jpg')
INSERT [dbo].[Images] ([ImageID], [ImageBoolID], [ImageURL]) VALUES (83, 89, N'/Content/files/220px-AnoHana_DVD_vol_1.jpg')
SET IDENTITY_INSERT [dbo].[Images] OFF
SET IDENTITY_INSERT [dbo].[OrdersBooks] ON 

INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (10, CAST(0x0000AA4E00DC7476 AS DateTime), 10, 4, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 1, 0)
INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (11, CAST(0x0000AA4E0104D026 AS DateTime), 10, 4, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 0, 0)
INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (12, CAST(0x0000AA4E01052D1D AS DateTime), 10, 4, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 1, 0)
INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (13, CAST(0x0000AA4E011CB90D AS DateTime), 10, 0, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 0, 1)
INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (14, CAST(0x0000AA4E011DC3D8 AS DateTime), 10, 0, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 0, 0)
INSERT [dbo].[OrdersBooks] ([OrderID], [FoundedDate], [UserID], [Status], [Address], [Phone], [Email], [FullName], [Paid], [Canceled]) VALUES (15, CAST(0x0000AA4E011DDAB5 AS DateTime), 10, 2, N'Thôn Khang Ninh, Đức nhân, Đức thọ', N'0123456                                           ', N'a@gmail.com                                       ', N'Phan', 0, 1)
SET IDENTITY_INSERT [dbo].[OrdersBooks] OFF
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (10, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (10, 78, 20000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (11, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (12, 1, 71000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (12, 2, 41000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (12, 3, 30000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (12, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (13, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (14, 2, 41000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (14, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (15, 2, 41000, 1, CAST(0xA93F0B00 AS Date), 0)
INSERT [dbo].[OrdersDetails] ([OrderID], [BookID], [Total], [Quantity], [CreatedDate], [Status]) VALUES (15, 4, 36000, 1, CAST(0xA93F0B00 AS Date), 0)
SET IDENTITY_INSERT [dbo].[Publishers] ON 

INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (1, N'Nhà Xuất Bản Văn Hóa Văn Nghệ', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20V%C4%83n%20H%C3%B3a%20V%C4%83n%20Ngh%E1%BB%87_.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (2, N'Nhà Xuất Bản Văn Học', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20V%C4%83n%20H%E1%BB%8Dc.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (3, N'Nhà Xuất Bản Trẻ', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20Tr%E1%BA%BB.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (4, N'Nhà Xuất Bản Tổng hợp TP.HCM', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20T%E1%BB%95ng%20h%E1%BB%A3p%20TP_HCM.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (5, N'Nhà Xuất Bản Hội Nhà Văn', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20H%E1%BB%99i%20Nh%C3%A0%20V%C4%83n.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (6, N'Nhà Xuất Bản Phụ Nữ', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20Ph%E1%BB%A5%20N%E1%BB%AF.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (7, N'Nhà Xuất Bản Thời Đại', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20Th%E1%BB%9Di%20%C4%90%E1%BA%A1i.jpg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (8, N'Nhà Xuất Bản Thế Giới', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20Th%E1%BA%BF%20Gi%E1%BB%9Bi.png', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (9, N'Nhà Xuất Bản Hồng Đức', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20H%E1%BB%93ng%20%C4%90%E1%BB%A9c.jpeg', 0)
INSERT [dbo].[Publishers] ([PublisherID], [Name], [Address], [Email], [Phone], [Note], [ImageURL], [isDeleted]) VALUES (10, N'Nhà Xuất Bản Lao Động', NULL, NULL, NULL, NULL, N'/Content/files/Nh%C3%A0%20Xu%E1%BA%A5t%20B%E1%BA%A3n%20Lao%20%C4%90%E1%BB%99ng.jpg', 0)
SET IDENTITY_INSERT [dbo].[Publishers] OFF
INSERT [dbo].[UserRoles] ([UserRoleID], [Title]) VALUES (1, N'Admin                                             ')
INSERT [dbo].[UserRoles] ([UserRoleID], [Title]) VALUES (2, N'User                                              ')
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Birthday], [Phone], [Email], [ImageURL], [UserRoleID], [isActivated], [Address]) VALUES (2, N'QuocTrung', N'pdrEaavYrMG6rhsygYErxw==', N'Lê Quốc Trung', CAST(0x133F0B00 AS Date), N'1646658251', N'quoctrung12312@gmail.com', NULL, 2, 1, N'ANON')
INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Birthday], [Phone], [Email], [ImageURL], [UserRoleID], [isActivated], [Address]) VALUES (3, N'Admin', N'pdrEaavYrMG6rhsygYErxw==', N'Lê Quốc Trung', CAST(0x991E0B00 AS Date), N'1646658251', N'quoctrung12312@gmail.com', NULL, 1, 1, N'191/6 Lê Văn Việt, F. Hiệp Phú, Quận 9')
INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Birthday], [Phone], [Email], [ImageURL], [UserRoleID], [isActivated], [Address]) VALUES (5, N'phan', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'phan', CAST(0xA73F0B00 AS Date), N'0123456123', N'o@gmail.com', N'', 2, 1, N'Thôn Khang Ninh, Đức nhân, Đức thọ')
INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Birthday], [Phone], [Email], [ImageURL], [UserRoleID], [isActivated], [Address]) VALUES (10, N'phu', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Phan', CAST(0xA73F0B00 AS Date), N'0123456', N'a@gmail.com', N'', 1, 1, N'Thôn Khang Ninh, Đức nhân, Đức thọ')
INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Birthday], [Phone], [Email], [ImageURL], [UserRoleID], [isActivated], [Address]) VALUES (11, N'2119775598069707', N'123456', N'PhúPhan', CAST(0xA83F0B00 AS Date), NULL, N'puskynight@gmail.com', N'https://graph.facebook.com/2119775598069707/picture', 2, 1, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[Authors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Authors_dbo.ImageBool_ImageBoolID] FOREIGN KEY([ImageBoolID])
REFERENCES [dbo].[ImageBool] ([ImageBoolID])
GO
ALTER TABLE [dbo].[Authors] CHECK CONSTRAINT [FK_dbo.Authors_dbo.ImageBool_ImageBoolID]
GO
ALTER TABLE [dbo].[AuthorsBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorsBooks_dbo.Authors_AuthorID] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[Authors] ([AuthorID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorsBooks] CHECK CONSTRAINT [FK_dbo.AuthorsBooks_dbo.Authors_AuthorID]
GO
ALTER TABLE [dbo].[AuthorsBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorsBooks_dbo.Books_BookID] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorsBooks] CHECK CONSTRAINT [FK_dbo.AuthorsBooks_dbo.Books_BookID]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.ImageBool_ImageBoolID] FOREIGN KEY([ImageBoolID])
REFERENCES [dbo].[ImageBool] ([ImageBoolID])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.ImageBool_ImageBoolID]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherID] FOREIGN KEY([PublisherID])
REFERENCES [dbo].[Publishers] ([PublisherID])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherID]
GO
ALTER TABLE [dbo].[CategoriesBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CategoriesBooks_dbo.Books_BookID] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoriesBooks] CHECK CONSTRAINT [FK_dbo.CategoriesBooks_dbo.Books_BookID]
GO
ALTER TABLE [dbo].[CategoriesBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CategoriesBooks_dbo.Categories_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoriesBooks] CHECK CONSTRAINT [FK_dbo.CategoriesBooks_dbo.Categories_CategoryID]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Comments_dbo.Books_BookID] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_dbo.Comments_dbo.Books_BookID]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Comments_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_dbo.Comments_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[FilterPrice]  WITH CHECK ADD  CONSTRAINT [FK_FilterPrice_Books] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
GO
ALTER TABLE [dbo].[FilterPrice] CHECK CONSTRAINT [FK_FilterPrice_Books]
GO
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Images_dbo.ImageBool_ImageBoolID] FOREIGN KEY([ImageBoolID])
REFERENCES [dbo].[ImageBool] ([ImageBoolID])
GO
ALTER TABLE [dbo].[Images] CHECK CONSTRAINT [FK_dbo.Images_dbo.ImageBool_ImageBoolID]
GO
ALTER TABLE [dbo].[OrdersBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrdersBooks_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OrdersBooks] CHECK CONSTRAINT [FK_dbo.OrdersBooks_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[OrdersDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrdersDetails_dbo.Books_BookID] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books] ([BookID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdersDetails] CHECK CONSTRAINT [FK_dbo.OrdersDetails_dbo.Books_BookID]
GO
ALTER TABLE [dbo].[OrdersDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrdersDetails_dbo.OrdersBooks_OrderID] FOREIGN KEY([OrderID])
REFERENCES [dbo].[OrdersBooks] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrdersDetails] CHECK CONSTRAINT [FK_dbo.OrdersDetails_dbo.OrdersBooks_OrderID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.UserRoles_UserRoleID] FOREIGN KEY([UserRoleID])
REFERENCES [dbo].[UserRoles] ([UserRoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.UserRoles_UserRoleID]
GO
USE [master]
GO
ALTER DATABASE [BSDataba] SET  READ_WRITE 
GO
