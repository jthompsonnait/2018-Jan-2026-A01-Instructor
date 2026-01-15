<Query Kind="Statements">
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

Albums
.OrderBy(a => a.ReleaseYear)
	.Select(a => new
	{
		Title = a.Title,
		Artist = a.Artist.Name,
		Year = a.ReleaseYear,
		Decades = a.ReleaseYear < 1970 ? "Oldies" :
						 a.ReleaseYear < 1980 ? "70's" :
						 a.ReleaseYear < 1990 ? "80's" :
						 a.ReleaseYear < 2000 ? "90's" : "Modern"
	})
	//.OrderBy(a => a.Decades)
	.ToList()
	.Dump();