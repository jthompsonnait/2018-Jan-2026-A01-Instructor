<Query Kind="Statements">
  <Connection>
    <ID>af9efb4e-acd2-4c19-b04d-92901e6e0f43</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

var artists = Artists.Select(artist => new ArtistView
{
	Name = artist.Name,
	Albums = artist.Albums.Select(album => new AlbumView
	{
		Title = album.Title,
		Year = album.ReleaseYear
	}).ToList()
}).ToList();

artists.Dump();
public class ArtistView
{
	public string Name { get; set; }
	public List<AlbumView> Albums { get; set; }
}
public class AlbumView
{
	public string Title { get; set; }
	public int Year { get; set; }
}