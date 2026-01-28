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
	//  could prefilter the release label for not including "nulls"
	.Where(album => album.ReleaseLabel != null)
	.GroupBy(album => new {album.ReleaseLabel, album.ReleaseYear})
	//	could filter here but you are processing the count twice.
	//.Where(g => g.Count() > 1)
	.Select(g => new
				{
					Year = g.Key.ReleaseYear,
					Label = g.Key.ReleaseLabel,
					Count = g.Count(),
					Albums = g.Select(a => new
					{
						Title = a.Title,
						Artist = a.Artist.Name
					})
					.OrderBy(a => a.Title)
				})
			.Where(g => g.Count > 1)
			.OrderBy(g => g.Year)
			.ThenBy(g => g.Label)
			.ToList()
			.Dump();