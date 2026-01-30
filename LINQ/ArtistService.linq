<Query Kind="Program">
  <Connection>
    <ID>5369a8b5-dcd6-4128-b2f6-d33725f2277b</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
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

	public Result<ArtistEditView> GetArtist(int artistID)
	{
		//	Create a Result container that will hold either a 
		//		ArtistEditView object on success or any accumulated errors on failure
		var result = new Result<ArtistEditView>();
		
		#region Business Logic and Parameter Exceptions
		//	Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data
		//	Rule:	artistID must be valid
		
		if(artistID == 0)
		{
			//	need to exit because we have nothing to search on
			return result.AddError(new Error("Missing Information", "Please provide a valid artist ID"));
		}
		#endregion
		
		//	artist that meet our criteria
		var artist = _hogWildContext.Artists
						.Where(a => a.ArtistId == artistID)
						.Select(a => new ArtistEditView
						{
							ArtistID = a.ArtistId,
							Name = a.Name
						}).FirstOrDefault();
						
		//	if no artist were found with the artist id provided
		if(artist == null)
		{
			result.AddError(new Error("No Artist", $"No artist was found with ID: {artistID}"));
			//	need to exit because we will not be able to add a null artist to
			//		the result if there are any errors
			return result;
		}
		
		//	return the result
		return result.WithValue(artist);
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class ArtistEditView
{
	public int ArtistID { get; set; }
	public string Name { get; set; }
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