USE [toolmanager]
GO
/****** Object:  StoredProcedure [dbo].[Message_GetByPage]    Script Date: 9/16/2022 2:50:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Message_GetByPage]
	@ConversationId int,
	@Offset int,
	@Id int,
	@Direction int,
	@PageSize int,
	@IsMore bit
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TotalCount int;
	DECLARE @Count int = 0;
	SET @TotalCount = (SELECT COUNT(1) FROM tbl_messages a WHERE 1=1 AND ConversationId = @ConversationId);

	IF @IsMore IS NULL OR @IsMore = 0
	BEGIN
		IF @Id > 0
		BEGIN
			IF @Direction = 1
			BEGIN				
				SET @Count = (SELECT COUNT(1) FROM tbl_messages a WHERE a.ConversationId = @ConversationId AND a.Id >= @Id);

				IF @Count < @PageSize
				BEGIN
					SELECT *
					INTO #MessagePaging
					FROM(
						SELECT Top(@PageSize - @Count) @TotalCount as TotalCount, a.Id, a.CreateDate, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
						WHERE a.ConversationId = @ConversationId AND a.Id < @Id
						ORDER BY a.CreateDate DESC

						UNION
						SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, a.CreateDate, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
						WHERE a.ConversationId = @ConversationId AND a.Id >= @Id
						ORDER BY a.CreateDate DESC
					) as x;

					SELECT * FROM #MessagePaging ORDER BY CreateDate DESC
					DROP TABLE #MessagePaging
				END
				ELSE
				BEGIN
					SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
					WHERE a.ConversationId = @ConversationId AND a.Id >= @Id
					ORDER BY a.CreateDate DESC
				END
			END

			ELSE IF @Direction = 2
			BEGIN
				SET @Count = (SELECT COUNT(1) FROM tbl_messages a WHERE a.ConversationId = @ConversationId AND a.Id <= @Id);

				IF @Count < @PageSize
				BEGIN
					SELECT *
					INTO #MessageDownPaging
					FROM(
						SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, a.CreateDate, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
						WHERE a.ConversationId = @ConversationId AND a.Id > @Id

						UNION

						SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, a.CreateDate, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
						WHERE a.ConversationId = @ConversationId AND a.Id <= @Id
						ORDER BY a.CreateDate DESC
					) as x;

					SELECT * FROM #MessageDownPaging ORDER BY CreateDate DESC
					DROP TABLE #MessageDownPaging
				END
				ELSE
				BEGIN
					SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
					WHERE a.ConversationId = @ConversationId AND a.Id <= @Id
					ORDER BY a.CreateDate DESC
				END
			END
		END
		ELSE 
		BEGIN
			SELECT @TotalCount as TotalCount, a.Id, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
			WHERE a.ConversationId = @ConversationId AND a.Status = 1
			ORDER BY a.CreateDate DESC
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY
		END
	END
	ELSE IF @IsMore = 1	
	BEGIN
		IF @Direction = 1
			SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
			WHERE a.ConversationId = @ConversationId AND a.Id >= @Id

		ELSE IF @Direction = 2
			SELECT Top(@PageSize) @TotalCount as TotalCount, a.Id, [dbo].F_Message_GetPageIndex(a.Id, @ConversationId, @PageSize, @TotalCount) AS PageIndex FROM tbl_messages a
			WHERE a.ConversationId = @ConversationId AND a.Id <= @Id
			ORDER BY a.CreateDate DESC
	END
END