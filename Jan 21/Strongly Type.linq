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
	List<SongView> results = GetSongsByPartialName("Dance");
	results.FirstOrDefault().Dump();
	results.Dump();
}

public List<SongView> GetSongsByPartialName(string partialSongName)
{
	return Tracks
		.Where(t => t.Name.ToLower().Contains(partialSongName.ToLower()))
		.Select(t => new SongView
		{
			AlbumTitle = t.Album.Title,
			SongTitle = t.Name,
			Artist = t.Album.Artist.Name 
		}).ToList();
}

public class SongView
{
	public string AlbumTitle { get; set; }
	public string SongTitle { get; set; }
	public string Artist { get; set; }
}