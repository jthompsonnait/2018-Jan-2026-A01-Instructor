<Query Kind="Statements">
  <Connection>
    <ID>22afda9d-35b4-4438-a6c6-ec4906c9a3ad</ID>
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
		.GroupBy(od => new {od.Order.Customer.City, od.Product.Category.CategoryName})
		.Select(g => new
		 {
			City = g.Key.City,
			Category = g.Key.CategoryName,
			Count = g.Count()
		})
		.OrderBy(g => g.City)
		.ThenBy(g => g.Category)
		.ToList()
		.Dump();
		