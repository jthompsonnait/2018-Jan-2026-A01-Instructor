<Query Kind="Statements">
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

//Question: "How would you filter the Inventories table to retrieve records for products in the 'Cell phones' 
//	category, and return the results as an anonymous data set that includes the store ID, 
//	store name, product name, and whether a reorder is necessary, 
//	ordered by store ID, then by ProductName?"

Inventories
	.Where(i => i.Product.ProductSubcategory.ProductCategory.ProductCategoryName == "Cell phones")	
	.Select(i => new
	{
		StoreID = i.StoreID,
		Store = i.Store.StoreName,
		ProductName = i.Product.ProductName,
		Manuf = i.Product.Manufacturer,
		Reorder = (i.OnHandQuantity + i.OnOrderQuantity) >= i.SafetyStockQuantity
					? "No" : "Yes"				
	})
	.OrderBy(i => i.Reorder)
	.ThenBy(i => i.ProductName)
	.ToList()
	.Dump();
	
	