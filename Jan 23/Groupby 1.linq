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

//Albums
//	.OrderBy(x => x.ReleaseYear)
//	.Select(x => x)
//	.ToList()
//	.Dump();
	
//Albums
//	.GroupBy(a => a.ReleaseYear)	
//	.Select(g => g)
//	//.ToList()
//	.Dump();

Albums
	.GroupBy(a => a.ReleaseYear)
	.Select(g => new
	{
		Year = g.Key,
		Albums = g.ToList()
	})
	.ToList()
	.Take(5)
	.Dump();
	
Artists
		.Select(a => new
		{
			Name = a.Name,
			Albums = a.Albums			
			.Select(ab => new
			{
				AlbumID = ab.AlbumId,
				Title = ab.Title
			})			
			.ToList()
		})
		.OrderBy(a => a.Name)
		.Take(6)
		.Dump();