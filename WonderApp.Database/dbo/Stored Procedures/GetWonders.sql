-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetWonders] 
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
	select * from Deals d --TABLESAMPLE (@take ROWS)
	join AspNetUsers u on u.Id = @userId
	join AspNetUserCategory c on c.UserId = @userId
	left join UserDealReject r on r.MyRejects_Id = d.Id
	left join UserDealWonders w on w.MyWonders_Id = d.Id
	where d.Archived = 0
	and d.Expired = 0
	and d.CityId = @cityId
	and d.Priority = @priority
	and (d.AlwaysAvailable = 1 or d.ExpiryDate >= GETDATE())
	and (d.Gender_Id = 0 or d.Gender_Id = u.Gender_Id)
	and d.Category_Id = c.Categories_Id
	and (r.MyRejectUsers_Id is null or r.MyRejectUsers_Id <> @userId)
	and (w.MyWonderUsers_Id  is null or w.MyWonderUsers_Id <> @userId)

END