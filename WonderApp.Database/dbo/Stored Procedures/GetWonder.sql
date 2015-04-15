-- =============================================
-- Author:		Graham McVea
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetWonder 
	-- Add the parameters for the stored procedure here
	@wonderId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
	from Deals d --TABLESAMPLE (@take ROWS)
	--company
	join Companies c on c.Id = d.Company_Id
	--location
	join Locations l on l.Id = d.Location_Id
	--cost
	join Costs co on co.Id = d.Cost_Id
	--category
	join Categories cat on cat.id = d.Category_Id
	--image
	join Images i on i.Deal_Id = d.Id
	--city
	join Cities ci on ci.Id = d.CityId
	--address
	join Addresses a on a.Id = d.AddressId
	--season
	join Seasons s on s.id = d.Season_Id
	--gender
	join Genders g on g.id = d.Gender_Id
	--list dealtag
	--list ages
	where d.Id = @wonderId
END