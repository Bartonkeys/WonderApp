-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetWonderTags 
	-- Add the parameters for the stored procedure here
	@userId nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select d.Id as DealId, 
	t.Id as TagId, 
	t.Name as TagName
	from aspnetUsers us
	join userdealwonders udw on udw.mywonderusers_Id = us.Id
	join deals d on d.Id = udw.mywonders_Id
	join DealTag dt on dt.Deals_Id = d.Id
	join Tags t on t.Id = dt.Tags_Id
	where us.Id = @userId

END