USE [toolmanager]
GO
/****** Object:  StoredProcedure [dbo].[Message_GetBySearch]    Script Date: 9/16/2022 3:44:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Message_GetBySearch]
@ConversationId int,
@Keyword nvarchar(128),
@PageSize int

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TotalCount int;

	SET @TotalCount = (SELECT COUNT(1) FROM tbl_messages a
	 	WHERE 1=1
		AND (@Keyword IS NULL OR a.Message LIKE '%' + @Keyword + '%' OR a.Message LIKE '%' + @Keyword + '%')
		AND a.ConversationId = @ConversationId AND a.Status = 1
	);
	
	SELECT @TotalCount as TotalCount, a.Id, a.ConversationId, a.Message, a.Type, a.SenderId, a.ReceiverId, a.CreateDate, a.Important, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex
		FROM tbl_messages a 
		WHERE 1=1
		AND (@Keyword IS NULL OR a.Message LIKE '%' + @Keyword + '%' OR a.Message LIKE '%' + @Keyword + '%')
		AND a.ConversationId = @ConversationId AND a.Status = 1
		ORDER BY a.CreateDate DESC
END