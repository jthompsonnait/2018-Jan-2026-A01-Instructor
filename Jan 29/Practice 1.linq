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

ProductSubcategories
	.GroupBy(psc => psc.ProductCategory.ProductCategoryName)
	.Select(g => new
	{
		CategoryName = g.Key,
		ProductSubCategories = g.Select(x => new
		{
			SubCategoryName = x.ProductSubcategoryName
		})
		.OrderBy(x => x.SubCategoryName)
		.ToList()
	})
	.ToList()
	.Dump();