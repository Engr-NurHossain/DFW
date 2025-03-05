SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UpdateCustomerFile]
(
    @Id int,
    @FileDescription nvarchar(max),
    @Filename nvarchar(500),
    @FileFullName nvarchar(500),
    @Uploadeddate datetime,
    @CustomerId uniqueidentifier,
    @CompanyId uniqueidentifier,
    @IsActive bit,
    @FileSize float,
    @Tag nvarchar(50),
    @InvoiceId nvarchar(50),
    @GeeseFileType nvarchar(50),
    @CreatedBy uniqueidentifier,
    @CreatedDate datetime,
    @UpdatedBy uniqueidentifier,
    @UpdatedDate datetime,
    @FileId uniqueidentifier,
    @WMStatus nvarchar(50),
    @AWSProcessStatus nvarchar(50),
    @AWSUploadTS datetime
)
AS
    UPDATE [dbo].[CustomerFile] 
    SET
    [FileDescription] = @FileDescription,
    [Filename] = @Filename,
    [FileFullName] = @FileFullName,
    [Uploadeddate] = @Uploadeddate,
    [CustomerId] = @CustomerId,
    [CompanyId] = @CompanyId,
    [IsActive] = @IsActive,
    [FileSize] = @FileSize,
    [Tag] = @Tag,
    [InvoiceId] = @InvoiceId,
    [GeeseFileType] = @GeeseFileType,
    [CreatedBy] = @CreatedBy,
    [CreatedDate] = @CreatedDate,
    [UpdatedBy] = @UpdatedBy,
    [UpdatedDate] = @UpdatedDate,
    [FileId] = @FileId,
    [WMStatus]= @WMStatus,
    [AWSProcessStatus]=@AWSProcessStatus,
    [AWSUploadTS]=@AWSUploadTS

    WHERE ( Id = @Id )

    DECLARE @Err int
    DECLARE @Result int
    SET @Result = @@ROWCOUNT
    SET @Err = @@ERROR 

    If @Err <> 0 
    BEGIN
        SET @Result = -1
    END

    RETURN @Result