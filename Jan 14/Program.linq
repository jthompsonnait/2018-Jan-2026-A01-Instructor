<Query Kind="Program">
  <Connection>
    <ID>e0a87a77-277f-494c-93a7-51c2205344d2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	//  method call
	GetAllAlbumsByYear(1999).Dump("For year 1999");
	GetAllAlbumsByYear(2000).Dump("For year 2000");
}

//	image this is a method in your BLL srever
public List<Album> GetAllAlbumsByYear(int year)
{
	return Albums
			.Where(a => a.ReleaseYear == year)
			.ToList();
}