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

Orders
	.GroupBy(o => o.Employee.FirstName + " " + o.Employee.LastName)
	.Select(g => new
	{
		Sales = g.Key,
		Orders = g.Select(o => new
		{
			OrderID = o.OrderID,
			Date =o.OrderDate,
			Customer = o.Customer.CompanyName
		})
		.Take(5)
	})
	.ToList()
	.Dump();