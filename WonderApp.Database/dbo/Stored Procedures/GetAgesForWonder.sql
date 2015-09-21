-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetAgesForWonder 
	-- Add the parameters for the stored procedure here
	@wonderId int
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
	where d.Id = @wonderId

END