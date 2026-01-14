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

Invoices
	.Select(i => new
	{
		Id = i.InvoiceId,
		Date = i.InvoiceDate.ToString(),
		ShortDate = i.InvoiceDate.ToShortDateString(),
		Customer = i.Customer.FirstName + " " + i.Customer.LastName
	})
	.Dump();