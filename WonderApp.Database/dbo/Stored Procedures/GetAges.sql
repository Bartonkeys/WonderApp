-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetAges] 
	-- Add the parameters for the stored procedure here
	@userId nvarchar(128),
	@cityId int,
	@priority int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select d.Id as DealId, 
	a.Id as AgeId, 
	a.Name as AgeName
	from DealAge da
	join Ages a on a.Id = da.Ages_Id
	join Deals d on d.Id = da.Deals_Id
	join AspNetUsers u on u.Id = @userId
	where d.Archived = 0
	and d.Expired = 0
	and d.CityId = @cityId
	and d.Priority = @priority
	and (d.AlwaysAvailable = 1 or d.ExpiryDate >= SysDateTime())
	and d.Gender_Id in (select Id from Genders where Name = 'All' or Id = u.Gender_Id)
	and d.Category_Id in (select Categories_Id from AspNetUserCategory where UserId = u.Id)
	AND ( NOT EXISTS (SELECT 
        1 AS [C1]
        FROM [dbo].[UserDealReject]
        WHERE d.Id = [MyRejects_Id] AND [MyRejectUsers_Id] = @userId) )
	AND ( NOT EXISTS (SELECT 
        1 AS [C1]
        FROM [dbo].[UserDealWonders]
        WHERE d.Id = [MyWonders_Id] AND [MyWonderUsers_Id] = @userId))

END