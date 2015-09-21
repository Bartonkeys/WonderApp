-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetMyWonders] 
	-- Add the parameters for the stored procedure here
	@userId nvarchar(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select 
	d.Id, 
	d.Title AS Title, 
	d.Description AS Description, 
	d.Url AS Url, 
	d.ExpiryDate AS ExpiryDate, 
	d.Likes AS Likes, 
	d.Publish AS Publish, 
	d.Archived AS Archived, 
	d.IntroDescription AS IntroDescription, 
	d.Priority AS Priority, 
	d.CityId AS CityId, 
	d.AlwaysAvailable AS AlwaysAvailable, 
	d.AddressId AS AddressId, 
	d.Creator_User_Id AS Creator_User_Id, 
	d.Season_Id AS Season_Id, 
	d.Phone AS Phone, 
	d.Gender_Id AS Gender_Id, 
	d.Expired AS Expired, 
	d.Broadcast As Broadcast,
	d.Company_Id AS Company_Id, 
	d.Location_Id AS Location_Id, 
	d.Cost_Id AS Cost_Id, 
	d.Category_Id AS Category_Id,
	 c.Name as CompanyName,
	 l.Geography.Lat as Latitude,
	 l.Geography.Long as Longitude,
	 co.Range,
	 cat.Name as CategoryName,
	 i.url as ImageURL,
	 ci.Name as CityName,
	 a.AddressLine1,
	 a.AddressLine2,
	 a.PostCode,
	 s.Name as Season,
	 g.Name as Gender 
	from aspnetUsers us
	join userdealwonders udw on udw.mywonderusers_Id = us.Id
	join deals d on d.Id = udw.mywonders_Id
	join Companies c on c.Id = d.Company_Id
	join Locations l on l.Id = d.Location_Id
	join Costs co on co.Id = d.Cost_Id
	join Categories cat on cat.id = d.Category_Id
	join Images i on i.Deal_Id = d.Id
	join Cities ci on ci.Id = d.CityId
	join Addresses a on a.Id = d.AddressId
	join Seasons s on s.id = d.Season_Id
	join Genders g on g.id = d.Gender_Id
	where us.Id = @userId

END