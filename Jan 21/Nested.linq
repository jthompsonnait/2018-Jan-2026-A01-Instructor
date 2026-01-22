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
// Q1
Artists
	.OrderBy(x => x.ArtistId)
	.Select(x => new ArtistView
	{
		Artist = x.Name,
		Albums = x.Albums
	//	.OrderBy(a => a.Title)
		.Select(a => new AlbumView
		{
			ID = a.AlbumId,
			Album = a.Title,
			Label = a.ReleaseLabel,
			Year = a.ReleaseYear,
			Tracks = a.Tracks
					// pre-calc
					// .Where(t => t.Milliseconds / 1000 > 250)
						.Select(t => new TrackView
						{
							TrackID = t.TrackId,
							Name = t.Name,
							Length = t.Milliseconds / 1000
						})
						// post-calc
						// .Where(t => t.Length > 250)
						.OrderBy(t => t.Name)
						.ToList()
		})
		.OrderBy(a => a.Album)
		.ToList()
	})
	.ToList()
	.Dump();
	
public class ArtistView
{
	public string Artist { get; set; }
	public List<AlbumView> Albums { get; set; }
}	

public class AlbumView
{
	public int ID { get; set; }
	public string Album { get; set; }
	public string Label { get; set; }
	public int Year { get; set; }
	public List<TrackView> Tracks { get; set; }

}

public class TrackView
{
	public int TrackID { get; set; }
	public string Name { get; set; }
	public int Length { get; set; }
}