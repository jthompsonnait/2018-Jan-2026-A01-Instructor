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

int paramYear = 2000;
//var selectM = Albums
//				.Where(a => a.ReleaseYear == paramYear)
//				.Select(a => a);
//selectM.Dump();

Albums
	.Where(a => a.ReleaseYear == paramYear)
	.Select(a => a).Dump();

List<Album> GetAlbumsByYear(int releaseYear)
{
	return Albums
			.Where(a => a.ReleaseYear == paramYear)
			.Select(a => a)
			.OrderBy(a => a.Title)
			.ToList();

}