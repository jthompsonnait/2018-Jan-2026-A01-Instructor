<Query Kind="Program">
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

void Main()
{
	GetProductCategories().Dump();
}

public List<ProductCategorySummaryView> GetProductCategories()
{
	return ProductCategories
			.Select(pc => new ProductCategorySummaryView
				{
					ProductCategoryName = pc.ProductCategoryName,
					SubCategories = pc.ProductSubcategories
						.Select(sc => new ProductSubCategorySummaryView
							{
								SubCategoryName = sc.ProductSubcategoryName,
								Description = sc.ProductSubcategoryDescription
							})
							.OrderBy(sc => sc.SubCategoryName)
							.ToList()
				})
				.OrderBy(pc => pc.ProductCategoryName)
				.ToList();
}

public class ProductCategorySummaryView
{
	public string ProductCategoryName { get; set; }
	public List<ProductSubCategorySummaryView> SubCategories { get; set; }
}

public class ProductSubCategorySummaryView
{
	public string SubCategoryName { get; set; }
	public string Description { get; set; }
}