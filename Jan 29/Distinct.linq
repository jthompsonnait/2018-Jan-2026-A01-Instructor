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
	.Where(x => x.ReleaseYear > 1970)
	.OrderBy(x => x.ReleaseYear)
	.ThenBy(x => x.ReleaseLabel)
	.Select(x => new
	{
		Year = x.ReleaseYear,
		Label = x.ReleaseLabel
	})
	.Distinct()
	.Dump();