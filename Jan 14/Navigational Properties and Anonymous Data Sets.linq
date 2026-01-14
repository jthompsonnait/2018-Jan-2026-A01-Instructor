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

Artists
	.Where(x => x.ArtistId < 6)
	.OrderBy(x => x.ArtistId)
	.Select(x => x)
	.Dump();

// album, label, year
//  order by Title
Albums
.Where(x => x.AlbumId < 10)
.OrderByDescending(x => x.AlbumId)
.Select(x => new
{
	Album = x.Title,
	Label = x.ReleaseLabel,
	Year = x.ReleaseYear,
	SubYear = (x.ReleaseYear / 100),
	ArtistName = x.Artist.Name
}
)
.OrderBy(a => a.Album)
.Dump();