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

//Artists	.Where(x => x.ArtistId <=5).Dump();
//Albums.Where(x => x.ArtistId <=5).OrderBy(x => x.ArtistId).Dump();

Artists
	.OrderBy(x => x.ArtistId)
	.Select(x => new
	{
		Artist = x.Name,
		MyFavoriteAlbums = x.Albums
	//	.OrderBy(a => a.Title)
		.Select(a => new
		{
			ID = a.AlbumId,
			Album = a.Title,
			Label = a.ReleaseLabel,
			Year = a.ReleaseYear
		})
		.OrderBy(a => a.Album)
		.ToList()
	})
	.ToList()
	.Dump();

Artists
.OrderBy(x => x.ArtistId)
.Select(x => new
{
	Artist = x.Name,
	MyFavoriteAlbums = x.Albums
	//	.OrderBy(a => a.Title)
	.Select(a => new
	{
		ID = a.AlbumId,
		Album = a.Title,
		Label = a.ReleaseLabel,
		Year = a.ReleaseYear
	})
	.OrderBy(a => a.Album)
	.ToList()
})
.ToList()
.Dump();


//Albums
//	.OrderBy(a => a.Title)
//	.Select(a => new
//	{
//		Album = a.Title,
//		Label = a.ReleaseLabel,
//		Year = a.ReleaseYear
//	})
//	.ToList()
//	.Dump();