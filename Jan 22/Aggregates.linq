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
	// .Where(x => x.Tracks.Count() > 10)
	.OrderBy(x => x.Title)
	.Select(x => new
	{
		Title = x.Title,
		Artist = x.Artist.Name,
		Count = x.Tracks.Count(),
		//Seconds = x.Tracks.Select(x => x.Milliseconds/ 1000),
		Time1 = x.Tracks.Sum(x => x.Milliseconds) / 1000,
		Time2 = x.Tracks.Select(x => x.Milliseconds).Sum() / 1000,
		MinTrackLength1 = x.Tracks.Min(x => x.Milliseconds) / 1000,
		MinTrackLength2 = x.Tracks.Select(x => x.Milliseconds).Min() / 1000,
		MaxTrackLength1 = x.Tracks.Max(x => x.Milliseconds) / 1000,
		MaxTrackLength2 = x.Tracks.Select(x => x.Milliseconds).Max() / 1000,
		AverageTrackLength1 = x.Tracks.Average(x => x.Milliseconds) / 1000,
		AverageTrackLength2 = x.Tracks.Select(x => x.Milliseconds).Average() / 1000,
	})
	//.Where(x => x.Count > 10)
	//.OrderBy(x => x.Count)
	.ToList()
	.Dump();