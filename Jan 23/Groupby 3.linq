<Query Kind="Statements">
  <Connection>
    <ID>17f0f6e0-07c2-4191-89cd-bceb147f3aee</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WestWind-2024</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

Customers
	.GroupBy(c => c.Region)	
	.Select(g => new
	{
		Region = g.Key == null ? "Unknown" : g.Key,
		OrderCount = g.Sum(c => c.Orders.Count())
	})
	.ToList()
	.Dump();