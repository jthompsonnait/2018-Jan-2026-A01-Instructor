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
	GetEmployeeReview("al", 30).Dump();
}

public List<EmployeeView> GetEmployeeReview(string lastName, decimal baseRate)
{
	return Employees
			.Where(e => e.LastName.ToLower().Contains(lastName.ToLower()))
			.OrderBy(e => e.LastName)
			.Select(e => new EmployeeView
			{
				// FullName = e.FirstName + " " + e.LastName
				FullName = $"{e.FirstName} {e.LastName}",
				Department = e.DepartmentName,
				IncomeCategory = e.BaseRate < baseRate
									? "Review Needed"
									: "No Review Needed"
			})
			.ToList();
}

public class EmployeeView
{
	public string FullName { get; set; }
	public string Department { get; set; }
	public string IncomeCategory { get; set; }
}