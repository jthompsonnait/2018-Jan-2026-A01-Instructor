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

//List all albums by release label.Any album with no label should be indicated as Unknown.
//List Title, Label, Artist Name, and Release Year.

Albums
	.Select(a => new
	{
		Title = a.Title,
		Label = a.ReleaseLabel == "" || a.ReleaseLabel == null ? "Unknown" : a.ReleaseLabel,
		Artist = a.Artist.Name,
		Year = a.ReleaseYear
	})
	.OrderBy(a => a.Label)
	.ToList()
	.Dump();