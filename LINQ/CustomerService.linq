<Query Kind="Program">
  <Connection>
    <ID>53bfbe22-8f63-4146-92e7-5b76f960a947</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>OLTP-DMIT2018</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
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

	public Result<CustomerEditView> GetCustomer(int customerID)
	{
		//	Create a Result container that will hold either a 
		//		CustomerEditView object on success or any accumulated errors on 
		//		failure.
		var result = new Result<CustomerEditView>();

		#region Business Rules
		//	These are processing rules that need to be satisfied for valid data
		//	rule:	customerID must be valid (cannot equal zero)
		//	rule:	RemoveFromViewFlag must be false (soft delete)

		if (customerID == 0)
		{
			//	need to exit because we have no customer ID
			result.AddError(new Error("Missing Information",
							"Please provide a valid customer ID"));
		}
		#endregion

		var customer = _hogWildContext.Customers
					.Where(c => c.CustomerID == customerID &&
							c.RemoveFromViewFlag == false)  //  !c.RemoveFromViewFlag 
					.Select(c => new CustomerEditView
					{
						CustomerID = c.CustomerID,
						FirstName = c.FirstName,
						LastName = c.LastName,
						Address1 = c.Address1,
						Address2 = c.Address2,
						City = c.City,
						ProvStateID = c.ProvStateID,
						CountryID = c.CountryID,
						PostalCode = c.PostalCode,
						Phone = c.Phone,
						Email = c.Email,
						StatusID = c.StatusID,
						RemoveFromViewFlag = c.RemoveFromViewFlag
					}).FirstOrDefault();
					
		// if no customer were found with the customer ID
		if(customer == null)
		{
			//	need to exit because we did not find any customer
			result.AddError(new Error("No Customer",
					$"No customer were found with customer ID :{customerID}"));
		}
		else
		{
			result.WithValue(customer);
		}
		
		//	return the result
		return result;
	}

}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class CustomerEditView
{
	public int CustomerID { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address1 { get; set; }
	public string Address2 { get; set; }
	public string City { get; set; }
	//	Prov/State ID.	Value will use a dropdown and the Lookup View Model
	public int ProvStateID { get; set; }
	//	Country ID.	Value will use a dropdown and the Lookup View Model
	public int CountryID { get; set; }
	public string PostalCode { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	//	Stauus ID.	Value will use a dropdown and the Lookup View Model
	public int StatusID { get; set; }
	//	soft delete
	public bool RemoveFromViewFlag { get; set; }
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