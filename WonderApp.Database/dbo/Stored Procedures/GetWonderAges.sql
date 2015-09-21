-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetWonderAges 
	-- Add the parameters for the stored procedure here
	@userId nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select d.Id as DealId, 
	a.Id as AgeId, 
	a.Name as AgeName
	from aspnetUsers us
	join userdealwonders udw on udw.mywonderusers_Id = us.Id
	join deals d on d.Id = udw.mywonders_Id
	join DealAge da on da.Deals_Id = d.Id
	join Ages a on a.Id = da.Ages_Id
	where us.Id = @userId

END