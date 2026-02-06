<Query Kind="Program">
  <Connection>
    <ID>65bbaa94-f03d-42a2-bf43-cbe67d7204a0</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>OLTP-DMIT2018</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
  <NuGetReference>BYSResults</NuGetReference>
</Query>

// 	Lightweight result types for explicit success/failure 
//	 handling in .NET applications.
using BYSResults;

// —————— PART 1: Main → UI ——————
//	Driver is responsible for orchestrating the flow by calling 
//	various methods and classes that contain the actual business logic 
//	or data processing operations.
void Main()
{
	CodeBehind codeBehind = new CodeBehind(this); // “this” is LINQPad’s auto Context
	
	//	Fail
	//	rule:	Both last name and phone number cannot be empty
	codeBehind.GetCustomers(string.Empty, string.Empty);
	codeBehind.ErrorDetails.Dump("Both last name and phone number cannot be empty");
	
	//	rule:	both the last name and/or phone number must be valid
	//			(no customers with last name of 'zzz' and phone number of 999999)
	codeBehind.GetCustomers("zzz", "999999");
	codeBehind.ErrorDetails.Dump("No customers with last name of 'zzz' and phone number of 999999");

}

// ———— PART 2: Code Behind → Code Behind Method ————
// This region contains methods used to test the functionality
// of the application's business logic and ensure correctness.
// NOTE: This class functions as the code-behind for your Blazor pages
#region Code Behind Methods
public class CodeBehind(TypedDataContext context)
{
	#region Supporting Members (Do not modify)
	// exposes the collected error details
	public List<string> ErrorDetails => errorDetails;

	// Mock injection of the service into our code-behind.
	// You will need to refactor this for proper dependency injection.
	// NOTE: The TypedDataContext must be passed in.
	private readonly Library YourService = new Library(context);
	#endregion

	#region Fields from Blazor Page Code-Behind
	// feedback message to display to the user.
	private string feedbackMessage = string.Empty;
	// collected error details.
	private List<string> errorDetails = new();
	// general error message.
	private string errorMessage = string.Empty;
	#endregion
	
	//	customer search view list returned by the service using GetCustomers()
	//	the visibility should be private in your code behind but we need to set
	//		it as public so that the Main()/driver can see it.
	public List<CustomerSearchView> Customers = default!;
	
	public void GetCustomers(string lastName, string phone)
	{
		//	clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = string.Empty;
		
		//	wrap the service call in a try/catch to handle unexpected exception
		try
		{
			var result = YourService.GetCustomers(lastName, phone);
			if(result.IsSuccess)
			{
				Customers = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch(Exception ex)
		{
			//	capture any exception message for display
			errorMessage = ex.Message;
		}
		
		
	}
}
#endregion

// ———— PART 3: Database Interaction Method → Service Library Method ————
//	This region contains support methods for testing
#region Methods
public class Library
{
	#region Data Context Setup
	// The LINQPad auto-generated TypedDataContext instance used to query and manipulate data.
	private readonly TypedDataContext _hogWildContext;

	// The TypedDataContext provided by LINQPad for database access.
	// Store the injected context for use in library methods
	// NOTE:  This constructor is simular to the constuctor in your service
	public Library(TypedDataContext context)
	{
		_hogWildContext = context
					?? throw new ArgumentNullException(nameof(context));
	}
	#endregion

	public Result<List<CustomerSearchView>> GetCustomers(string lastName, string phone)
	{
		//	Create a Result container that will hold either a 
		//		list of CustomerSearchView on success or any accumulated errors on failure
		var result = new Result<List<CustomerSearchView>>();

		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data

		//	rule:	Both last name and phone number cannot be empty
		//	rule:	RemoveFromViewFlag must be false (soft delete, soft rule)

		if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
		{
			//	need to exit because we have nothing to search on
			return result.AddError(new Error("Missing Information",
							"Please provice either a last name and/or phone number"));
		}
		#endregion

		//	filter rules
		//	1)	only apply lastName filter if supplied
		//	2) 	only apply phone filter if supplied
		//	3)	always exclude removed records

		var customers = _hogWildContext.Customers
							.Where(c => (string.IsNullOrWhiteSpace(lastName)
										|| c.LastName.ToUpper().Contains(lastName.ToUpper()))  // #1
									&& (string.IsNullOrWhiteSpace(phone)
										|| c.Phone.Contains(phone))	// #2
									&& c.RemoveFromViewFlag	== false	//	#3
									)
							.Select(c => new CustomerSearchView
								{
									CustomerID = c.CustomerID,
									FirstName = c.FirstName,
									LastName = c.LastName,
									City = c.City,
									Phone = c.Phone,
									Email = c.Email,
									StatusID = c.StatusID,
									//	if you have a nullable field, use the following pattern
									//		assume that the i.SubTotal is nullable in the table
									//	TotalSales = c.Invoices.Sum(i => (decial?)i.SubTotal + i.Tax) ?? 0
									//	if we have no invoices, the TotalSales will be null.  
									//		we need to add the ?? to handle null-> 0
									TotalSales = c.Invoices.Sum(i => i.SubTotal + i.Tax)
								})
								.OrderBy(c => c.LastName)
								.ToList();
		
		//	if no customer wer found with either the last name or phone
		if(customers == null || customers.Count() == 0)
		{
			//	need to exit because we did not find any customers
			return result.AddError(new Error("No Customers", "No customers were found"));
		}		
		
		//	return the result
		return result.WithValue(customers);
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class CustomerSearchView
{
	//	Customer ID
	public int CustomerID { get; set; }
	//	First name
	public string FirstName { get; set; }
	//	Last name
	public string LastName { get; set; }
	//	City
	public string City { get; set; }
	//	Contact phone number
	//	handles 011-xxxx or (780) 555-1212 ext 212
	public string Phone { get; set; }
	//	Email address
	public string Email { get; set; }
	//	Status ID.  Status value will use a dropdown and the "LookupView" model
	public int StatusID { get; set; }
	//	Invoice.Subtotal + Invoice.Tax
	public decimal TotalSales { get; set; }	
}
#endregion

//	This region includes support methods
#region Support Method
// Converts a list of error objects into their string representations.
public static List<string> GetErrorMessages(List<Error> errorMessage)
{
	// Initialize a new list to hold the extracted error messages
	List<string> errorList = new();

	// Iterate over each Error object in the incoming list
	foreach (var error in errorMessage)
	{
		// Convert the current Error to its string form and add it to errorList
		errorList.Add(error.ToString());
	}

	// Return the populated list of error message strings
	return errorList;
}
#endregion