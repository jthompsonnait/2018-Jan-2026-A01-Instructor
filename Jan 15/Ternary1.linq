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

//	(Company Name, Country & Fax). 
//	If the fax is empty (null) or blank, 
//	replace the “null” or blank value with “Unknown.”

Customers
	.Select(c => new
	{
		Name = c.CompanyName,
		Country = c.Country,
	 Fax = (c.Fax == "" || c.Fax == null) ? "Unknown" : c.Fax
		//Fax1 = string.IsNullOrWhiteSpace(c.Fax) ? "Unknown" : c.Fax
	})
	.OrderBy(c => c.Fax)
	//.ToList()
	.Dump();