<Query Kind="Statements">
  <Connection>
    <ID>e911c88b-78d8-48b8-9b71-c4628c7c6afb</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

Customers
.Where(x => x.TotalChildren > 2)
	.Sum(x => x.TotalChildren).Dump();

Products
.Take(100)
	.Select(x => new
	{
		Name = x.ProductName,
		QtyOnHand = x.Inventories.Sum(i => i.OnHandQuantity) == null
						? 0
						: x.Inventories.Sum(i => i.OnHandQuantity)
	})
	.OrderBy(x => x.Name)
	.ToList()
	.Dump();
