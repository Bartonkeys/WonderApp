-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetTagsForWonder 
	-- Add the parameters for the stored procedure here
	@wonderId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select d.Id as DealId, 
	t.Id as TagId, 
	t.Name as TagName
	from DealTag dt 
	join Tags t on t.Id = dt.Tags_Id
	join Deals d on d.Id = dt.Deals_Id
	where d.Id = @wonderId

END