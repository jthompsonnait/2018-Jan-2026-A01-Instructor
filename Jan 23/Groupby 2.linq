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

//Products
//	.GroupBy(p => p.Category.CategoryID)
//	.Select(g => new
//	{
//		Categories = g.Key,
//		Product = g.ToList()
//	})
//	.ToList()
//	.Dump();

//Categories
//	.Select(c => new
//	{
//		Categories = c.CategoryID,
//		Product = c.Products.Select(p => new
//		{
//			ProductId = p.ProductID,
//			ProductName = p.ProductName

//		})
//		.ToList()
//		.Take(5)
//	})
//	.ToList()
//	.Take(5)
//	.Dump();

//	group by category ID
Products
	 .GroupBy(p => p.Category.CategoryID)
	.Select(g => new
	{
		Key_CategoryID = g.Key,
		Categories = (Categories.Where(c => c.CategoryID == g.Key)
						.Select(c => c.CategoryName).FirstOrDefault()),
		Product = g.Select(p => new
		{
			ProductId = p.ProductID,
			ProductName = p.ProductName

		})
		.ToList()
	})
	.ToList()
	.Dump();

//	group by category name	
Products
	 .GroupBy(p => p.Category.CategoryName)
	.Select(g => new
	{
		Key_CategoryName = g.Key,
		Categories = g.Key,
		Product = g.Select(p => new
		{
			ProductId = p.ProductID,
			ProductName = p.ProductName

		})
		.ToList()
	})
	.ToList()
	.Dump();

