--===================================== STORE PROCEDURE ======================================
--=========================== START: MENU SERVICE ================================

/*INSERT*/
CREATE PROCEDURE [dbo].[Menu_Service_Insert]
	@Code varchar(20),	
	@Name nvarchar(128),
	@Description nvarchar(256),
	@CreatedBy int,
	@Status smallint
AS
BEGIN
	 DECLARE @newId int = 0;
	 INSERT INTO dbo.[tbl_menu](Code, [Name], [Description], CreatedBy, CreatedDate ,[Status]) 
	 VALUES(@Code, @Name, @Description, @CreatedBy, GETUTCDATE(), @Status);
END
/*UPDATE*/
GO
CREATE PROCEDURE [dbo].[Menu_Service_Update]
	@Id int,
	@Code varchar(20),	
	@Name nvarchar(128),
	@Description nvarchar(256),
	@LastUpdatedBy int,
	@Status smallint
AS
BEGIN

	 DECLARE @newId int = 0;
	 BEGIN TRANSACTION
	 BEGIN TRY 
		UPDATE dbo.[tbl_menu]
		SET
			Code = @Code,
			Name = @Name,
			Description = @Description,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedDate = GETUTCDATE(),
			Status = @Status
		WHERE 1 = 1
		AND Id = @Id;
		END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH
	IF @@TRANCOUNT > 0
	BEGIN 
		COMMIT TRANSACTION
	END
	SELECT @Id
END
GO	
/*GET BY PAGE*/
CREATE PROCEDURE [dbo].[Menu_Service_GetByPage]
	 @Keyword nvarchar(128),
	 @Status int,
	 @Offset int,
	 @PageSize int
AS
BEGIN
	 SET NOCOUNT ON;

	 DECLARE @TotalCount int;
	 SET @TotalCount = (SELECT COUNT(1) FROM dbo.[tbl_menu] a
	 	 WHERE 1=1
		 AND (a.Status = CASE WHEN (@Status = -1 OR @Status IS NULL) THEN a.Status ELSE @Status END)
		 AND (@Keyword IS NULL OR a.Name LIKE '%' + @Keyword + '%' OR a.Code LIKE '%' + @Keyword + '%')
		 AND a.[Status] != 9
	 );

	 SELECT @TotalCount as TotalCount, a.*
	 	 FROM dbo.[tbl_menu] a
	 	 WHERE 1=1
		 AND (a.Status = CASE WHEN (@Status = -1 OR @Status IS NULL) THEN a.Status ELSE @Status END)
		 AND (@Keyword IS NULL OR a.Name LIKE '%' + @Keyword + '%' OR a.Code LIKE '%' + @Keyword + '%')
		 AND a.[Status] != 9
	 	 ORDER BY a.Id DESC
	 	 OFFSET @Offset ROWS
	 	 FETCH NEXT @PageSize ROWS ONLY
	 ;
END
GO
/*GET BY ID*/
CREATE PROCEDURE [dbo].[Menu_Service_GetById]
	@Id int	
AS
BEGIN
	SELECT TOP 1 * FROM dbo.[tbl_menu] WHERE 1=1 AND Id=@Id
END
GO
/*DELETE*/
CREATE PROCEDURE [dbo].[Menu_Service_Delete]
	@Id int	
AS
BEGIN
	UPDATE dbo.[tbl_menu]
	SET Status = 9
	WHERE 1=1 AND Id=@Id
END
GO
--=========================== END: MENU SERVICE ================================

--=========================== START: WORKFLOW ================================
/*INSERT*/
CREATE PROCEDURE [dbo].[WorkFlow_Insert]
	@Code varchar(20),	
	@Name nvarchar(128),
	@Description nvarchar(256),
	@CreatedBy int,
	@Status smallint
AS
BEGIN 
	INSERT INTO [dbo].[tbl_workFlow](Code, [Name], [Description], CreatedBy, CreatedDate ,[Status]) 
	VALUES (@Code, @Name, @Description, @CreatedBy, GETUTCDATE(), @Status)	
END
GO
/*UPDATE*/
CREATE PROCEDURE [dbo].[WorkFlow_Update]
	@Id int,
	@Code varchar(20),	
	@Name nvarchar(128),
	@Description nvarchar(256),
	@LastUpdatedBy int,
	@Status smallint
AS
BEGIN

	 DECLARE @newId int = 0;
	 BEGIN TRANSACTION
	 BEGIN TRY 
		UPDATE dbo.[tbl_workFlow]
		SET
			Code = @Code,
			Name = @Name,
			Description = @Description,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedDate = GETUTCDATE(),
			Status = @Status
		WHERE 1 = 1
		AND Id = @Id;
		END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH
	IF @@TRANCOUNT > 0
	BEGIN 
		COMMIT TRANSACTION
	END
	SELECT @Id
END
GO	
/*GET BY PAGE*/
CREATE PROCEDURE [dbo].[WorkFlow_GetByPage]
	 @Keyword nvarchar(128),
	 @Status int,
	 @Offset int,
	 @PageSize int
AS
BEGIN
	 SET NOCOUNT ON;

	 DECLARE @TotalCount int;
	 SET @TotalCount = (SELECT COUNT(1) FROM dbo.[tbl_workflow] a
	 	 WHERE 1=1
		 AND (a.Status = CASE WHEN (@Status = -1 OR @Status IS NULL) THEN a.Status ELSE @Status END)
		 AND (@Keyword IS NULL OR a.Name LIKE '%' + @Keyword + '%' OR a.Code LIKE '%' + @Keyword + '%')
		 AND a.[Status] != 9
	 );

	 SELECT @TotalCount as TotalCount, a.*
	 	 FROM dbo.[tbl_workflow] a
	 	 WHERE 1=1
		 AND (a.Status = CASE WHEN (@Status = -1 OR @Status IS NULL) THEN a.Status ELSE @Status END)
		 AND (@Keyword IS NULL OR a.Name LIKE '%' + @Keyword + '%' OR a.Code LIKE '%' + @Keyword + '%')
		 AND a.[Status] != 9
	 	 ORDER BY a.Id DESC
	 	 OFFSET @Offset ROWS
	 	 FETCH NEXT @PageSize ROWS ONLY
	 ;
END
GO
/*GET BY ID*/
CREATE PROCEDURE [dbo].[WorkFlow_GetById]
	@Id int	
AS
BEGIN
	SELECT TOP 1 * FROM dbo.[tbl_workFlow] WHERE 1=1 AND Id=@Id
END
GO
--=========================== END: WORKFLOW ================================

--=========================== START: FORM ================================
/****** Object:  StoredProcedure [dbo].[Form_InsertFormField]    Script Date: 6/8/2022 10:16:16 AM ******/

CREATE PROCEDURE [dbo].[Form_InsertFormField]
	@FormId int,	
	@Template nvarchar(2000)
AS
BEGIN

	 DECLARE @newId int = 0;
	 INSERT INTO dbo.[tbl_form_filed](FormId,Template) 
	 VALUES(@FormId, @Template);
END
GO

/****** Object:  StoredProcedure [dbo].[Form_GetById]    Script Date: 6/8/2022 10:16:16 AM ******/
CREATE PROCEDURE [dbo].[Form_GetById]
	@Id int	
AS
BEGIN
	SELECT TOP 1 * FROM dbo.[tbl_form] WHERE 1=1 AND Id=@Id
END
GO

/****** Object:  StoredProcedure [dbo].[Form_GetByPage]    Script Date: 6/8/2022 10:16:16 AM ******/
CREATE PROCEDURE [dbo].[Form_GetByPage]
	 @Offset int,
	 @PageSize int
AS
BEGIN
	 SET NOCOUNT ON;

	 DECLARE @TotalCount int;
	 SET @TotalCount = (SELECT COUNT(1) FROM dbo.[tbl_form] a
	 	 WHERE 1=1
		 AND a.[Status] != 9
	 );

	 SELECT @TotalCount as TotalCount, a.*
	 	 FROM dbo.[tbl_form] a
	 	 WHERE 1=1
		 AND a.[Status] != 9
	 	 ORDER BY a.Id DESC
	 	 OFFSET @Offset ROWS
	 	 FETCH NEXT @PageSize ROWS ONLY
	 ;
END
GO

/****** Object:  StoredProcedure [dbo].[Form_GetTemplateByFormId]    Script Date: 6/8/2022 10:16:16 AM ******/
CREATE PROCEDURE [dbo].[Form_GetTemplateByFormId]
	@Id int	
AS
BEGIN
	SELECT Template FROM dbo.[tbl_form_filed] WHERE 1=1 AND FormId=@Id
END
GO

/****** Object:  StoredProcedure [dbo].[Form_Insert]    Script Date: 6/8/2022 10:16:16 AM ******/
CREATE PROCEDURE [dbo].[Form_Insert]
	@Code varchar(20),	
	@Name nvarchar(128),	
	@ShortDescription nvarchar(256),
	@CreatedBy int,
	@Status smallint
AS
BEGIN

	 DECLARE @newId int = 0;
	 INSERT INTO dbo.[tbl_form](Code,[Name],ShortDescription,CreatedBy,CreatedDate,[Status]) 
	 VALUES(@Code,@Name,@ShortDescription,@CreatedBy,GETUTCDATE(),1);
	 SET @newId = (SELECT SCOPE_IDENTITY());

	 SELECT @newId;
END
GO

/****** Object:  StoredProcedure [dbo].[Form_Update]    Script Date: 6/8/2022 10:16:16 AM ******/
CREATE PROCEDURE [dbo].[Form_Update]
	@Id int,
	@Code varchar(20),	
	@Name nvarchar(128),	
	@ShortDescription nvarchar(256),
	@LastUpdatedBy int,
	@Status smallint
AS
BEGIN

	 DECLARE @newId int = 0;
	 BEGIN TRANSACTION
	 BEGIN TRY 
		UPDATE dbo.[tbl_form]
		SET
			Code = @Code,
			Name = @Name,
			ShortDescription = @ShortDescription,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedDate = GETUTCDATE(),
			Status = @Status
		WHERE 1 = 1
		AND Id = @Id;
		END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH
	IF @@TRANCOUNT > 0
	BEGIN 
		COMMIT TRANSACTION
	END
	SELECT @Id
END
GO
--=========================== END: FORM ================================

CREATE PROCEDURE [dbo].[FormFiled_Update]
	@Id int,
	@Template nvarchar(MAX)
AS
BEGIN
	 DECLARE @newId int = 0;
	 BEGIN TRANSACTION
	 BEGIN TRY 
		UPDATE dbo.[tbl_form_filed]
		SET
			Template = @Template
		WHERE 1 = 1
		AND Id = @Id;
		END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH
	IF @@TRANCOUNT > 0
	BEGIN 
		COMMIT TRANSACTION
	END
	SELECT @Id
END
GO
---=========================================================
CREATE PROCEDURE [dbo].[FormFiled_Delete]
	@Id int
AS
BEGIN
	 DECLARE @newId int = 0;
	 BEGIN TRANSACTION
	 BEGIN TRY 
		DELETE dbo.[tbl_form_filed]
		WHERE 1 = 1
		AND Id = @Id;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH
	IF @@TRANCOUNT > 0
	BEGIN 
		COMMIT TRANSACTION
	END
END