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
	.Where(x => x.AlbumId > 0)
	.FirstOrDefault().Dump("With Where");

Albums
.OrderByDescending(a => a.AlbumId)
.FirstOrDefault().Dump("Without Where");

Albums
	.Where(x => x.AlbumId == -1)
	.FirstOrDefault().Dump("With invalid album id");