<Query Kind="Expression">
  <Connection>
    <ID>a3db8247-b6f6-4189-8113-e14d9b40f7cd</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

Invoices
//.Where(i => i.Customer.Geography.CityName == "Edmonton")
.OrderBy(i => i.Customer.LastName)
.ThenBy(i => i.Customer.FirstName)
.ThenBy(i => i.Store.StoreName)
	.Select(i => new
	{
		InvoiceNo = i.InvoiceID,
		Name = i.Customer.FirstName + " " + i.Customer.LastName,
		StoreName = i.Store.StoreName
	})
	
	
	