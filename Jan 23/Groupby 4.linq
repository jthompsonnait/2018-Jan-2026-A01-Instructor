<Query Kind="Expression">
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

OrderDetails
	.GroupBy(od => od.Product.ProductName)
	.Select(g => new
	{
		Product = g.Key,
		TotalQuantity = g.Sum(o1 => o1.Quantity),
		TotalSales = g.Sum(o1 => o1.Quantity * o1.UnitPrice)
	})
	.ToList()
	.Dump()