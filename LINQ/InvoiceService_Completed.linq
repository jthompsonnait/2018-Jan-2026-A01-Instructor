<Query Kind="Program">
  <Connection>
    <ID>813ec320-8be0-4b91-8ec8-c1549d53aaea</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>OLTP-DMIT2018</Database>
    <DisplayName>OLTP-DMIT2018-Entity</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
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

	//	invoices
	bool getCustomerInvoice = false;
	bool getInvoice = false;

	//  parts
	bool getParts = false;
	bool getPart = false;

	//  add invoice
	bool addInvoice = true;

	//	edit invoice
	bool editInvoice = true;

	#region GetCustomerInvoice
	if (getCustomerInvoice)
	{
		//	Fail
		//	Rule:  customer ID must be greater than zero
		codeBehind.GetCustomerInvoices(0);
		codeBehind.ErrorDetails.Dump("Customer ID must be greater than zero");

		// Rule:  customer ID must valid 
		codeBehind.GetCustomerInvoices(1000000);
		codeBehind.ErrorDetails.Dump("No customer was found for ID 1000000");

		// Pass:  valid customer ID
		codeBehind.GetCustomerInvoices(1);
		codeBehind.CustomerInvoices.Dump("Pass - Valid customer ID");
	}
	#endregion

	#region GetInvoice
	if (getInvoice)
	{
		//	Fail:
		//	Rule:	Customer and Invoice ID must be provided
		codeBehind.GetInvoice(0, 0, 1);
		codeBehind.ErrorDetails.Dump("Customer and InvoiceID must be greater than zero");

		//	Rule:	Employee ID must be provided
		codeBehind.GetInvoice(0, 1, 0);
		codeBehind.ErrorDetails.Dump("EmployeeID must be greater than zero");

		//	Pass:	New Invoice
		codeBehind.GetInvoice(0, 1, 1);
		codeBehind.Invoice.Dump("Pass - New Invoice");

		//	Pass:	Existing Invoice
		codeBehind.GetInvoice(1, 1, 1);
		codeBehind.Invoice.Dump("Pass - Existing Invoice");
	}
	#endregion

	#region GetParts
	if (getParts)
	{
		//	create a place holder for existing parts
		List<int> existingPartsIDs = new();

		//	Fail
		//	Rule: CategoryID & descripotion must be provided
		codeBehind.GetParts(0, string.Empty, existingPartsIDs);
		codeBehind.ErrorDetails.Dump("Category ID & description must be provided");

		//	Rule: No parts found
		codeBehind.GetParts(0, "zzz", existingPartsIDs);
		codeBehind.ErrorDetails.Dump("No parts were found that contain description 'zzz'");

		//	Pass:	valid part category ID (23 -> "Parts")
		codeBehind.GetParts(23, string.Empty, existingPartsIDs);
		codeBehind.Parts.Dump("Pass - Valid parts category ID");

		//	Pass:	valid partial description ('ra');
		codeBehind.GetParts(0, "ra", existingPartsIDs);
		codeBehind.Parts.Dump("Pass - Valid partial description");

		//	Pass: Updated existing parts ids
		existingPartsIDs.Add(27); //	Brake Oil, pint
		existingPartsIDs.Add(33); //	Transmission fuild, quart
		codeBehind.GetParts(0, "ra", existingPartsIDs);
		codeBehind.Parts.Dump("Pass - Valid partial description with existing parts ids");
	}
	#endregion

	#region GetPart
	if (getPart)
	{
		//	Fail
		//	Rule:  part ID must be greater than zero
		codeBehind.GetPart(0);
		codeBehind.ErrorDetails.Dump("Part ID must be greater than zero");

		// Rule:  part ID must valid 
		codeBehind.GetPart(1000000);
		codeBehind.ErrorDetails.Dump("No part was found for ID 1000000");

		// Pass:  valid part ID
		codeBehind.GetPart(52);
		codeBehind.Part.Dump("Pass - Valid part ID");
	}
	#endregion

	#region Add New Invoice
	InvoiceView invoiceView = null;
	InvoiceLineView invoiceLineView = null;
	if (addInvoice)
	{
		// Fail
		// invoice cannot be null
		codeBehind.AddEditInvoice(null);
		codeBehind.ErrorDetails.Dump("Invoice cannot is null");

		// setup Add Invoice
		invoiceView = new InvoiceView();

		// rule: customer id must be supply
		// rule: employee id must be supply    
		// rule: there must be invoice lines provided
		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.ErrorDetails.Dump("Fail - All rules except rules involving invoice lines");

		//  update missing fields
		invoiceView.CustomerID = 1;
		invoiceView.EmployeeID = 1;

		// rule: missing part
		invoiceLineView = new();
		invoiceView.InvoiceLines.Add(invoiceLineView);
		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.ErrorDetails.Dump("Fail - Missing part");

		// rule: price is less than zero
		// rule: quantity is less than one
		invoiceView.InvoiceLines[0].PartID = 1;
		invoiceView.InvoiceLines[0].Price = -1;
		invoiceView.InvoiceLines[0].Quantity = 0;

		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.ErrorDetails.Dump("Fail - Part price and quantity");

		// reset invoice line to correct value
		invoiceView.InvoiceLines[0].Price = 15;
		invoiceView.InvoiceLines[0].Quantity = 10;

		// rule: duplicated parts	
		invoiceLineView = new();
		invoiceLineView.PartID = 1;
		invoiceLineView.Price = 10;
		invoiceLineView.Quantity = 4;
		invoiceView.InvoiceLines.Add(invoiceLineView);
		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.ErrorDetails.Dump("Fail - Duplicated parts");

		// Pass:  valid new invoice
		// update duplicate part to unique part id
		invoiceView.InvoiceLines[1].PartID = 2;
		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.Invoice.Dump("Pass - New invoice");
	}

	// Pass:  edit invoice
	if (editInvoice)
	{
		// get last invoice ID
		int invoiceID = Invoices.OrderByDescending(i => i.InvoiceID)
						.Select(i => i.InvoiceID).FirstOrDefault();
		//  get the last invoice
		codeBehind.GetInvoice(invoiceID, 0, 1);
		invoiceView = codeBehind.Invoice;
		invoiceView.CustomerID = 4; // update customer 
		invoiceView.EmployeeID = 3; // update employee
									// update first invoice line quantity
		invoiceView.InvoiceLines[0].Quantity = 22;
		//add a new invoice line
		invoiceLineView = new();
		invoiceLineView.PartID = 3;
		invoiceLineView.Price = 88;
		invoiceLineView.Quantity = 121;
		invoiceView.InvoiceLines.Add(invoiceLineView);
		codeBehind.AddEditInvoice(invoiceView);
		codeBehind.Invoice.Dump("Pass - Updated invoice");
	}
	#endregion

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

	//	invoice view returned by the service
	//	using both the GetInvoice() & AddEditInvoice()
	public InvoiceView Invoice = default!;

	//	using GetCustomerInvoices
	public List<InvoiceView> CustomerInvoices = new();

	//	part return from the service
	//	using GetParts()
	public List<PartView> Parts = default!;

	//	using GetPart
	public PartView Part = default!;

	public void GetCustomerInvoices(int customerID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetCustomerInvoices(customerID);
			if (result.IsSuccess)
			{
				CustomerInvoices = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}

	public void GetInvoice(int invoiceID, int customerID, int employeeID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetInvoice(invoiceID, customerID, employeeID);
			if (result.IsSuccess)
			{
				Invoice = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}

	public void GetParts(int partCategoryID, string description, List<int> existingPartIDs)
	{
		//	clear previous error details and message
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = string.Empty;

		//wrap the service cal in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetParts(partCategoryID, description, existingPartIDs);
			if (result.IsSuccess)
			{
				Parts = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exceptions message for display
			errorMessage = ex.Message;
		}
	}

	public void GetPart(int partID)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.GetPart(partID);
			if (result.IsSuccess)
			{
				Part = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
			errorMessage = ex.Message;
		}
	}


	public void AddEditInvoice(InvoiceView invoiceView)
	{
		// clear previous error details and messages
		errorDetails.Clear();
		errorMessage = string.Empty;
		feedbackMessage = String.Empty;

		// wrap the service call in a try/catch to handle unexpected exceptions
		try
		{
			var result = YourService.AddEditInvoice(invoiceView);
			if (result.IsSuccess)
			{
				Invoice = result.Value;
			}
			else
			{
				errorDetails = GetErrorMessages(result.Errors.ToList());
			}
		}
		catch (Exception ex)
		{
			// capture any exception message for display
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

	//	Get the customer invoices
	public Result<List<InvoiceView>> GetCustomerInvoices(int customerId)
	{
		// Create a Result container that will hold either a
		//	PartView objects on success or any accumulated errors on failure
		var result = new Result<List<InvoiceView>>();
		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		rule:	customerID must be valid
		//		rule: 	RemoveFromViewFlag must be false
		if (customerId == 0)
		{
			result.AddError(new Error("Missing Information",
							"Please provide a valid customer id"));
			//  need to exit because we have no customer information
			return result;
		}
		#endregion

		var customerInvoices = _hogWildContext.Invoices
				.Where(x => x.CustomerID == customerId
							&& !x.RemoveFromViewFlag)
				.Select(x => new InvoiceView
				{
					InvoiceID = x.InvoiceID,
					InvoiceDate = x.InvoiceDate,
					CustomerID = x.CustomerID,
					SubTotal = x.SubTotal,
					Tax = x.Tax
				}).ToList();

		//  if no invoices were found
		if (customerInvoices == null || customerInvoices.Count() == 0)
		{
			result.AddError(new Error("No customer invoices", "No invoices were found"));
			//  need to exit because we did not find any invoices
			return result;
		}
		//  return the result
		return result.WithValue(customerInvoices);
	}

	//	Get a invoice
	public Result<InvoiceView> GetInvoice(int invoiceID, int customerID, int employeeID)
	{
		//	Create a Result container that will hold either a
		//	  InvoiceView object on success or any accumulated errors on failure
		var result = new Result<InvoiceView>();

		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data
		//		rule:  cusomerID must be provided if invoiceID == 0
		//		Rule:  employeeID must be provided
		if (customerID == 0 && invoiceID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a customer ID and/or invoice ID"));
		}

		if (employeeID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a employee ID"));
		}

		// need to exit because we are missing key data
		if (result.IsFailure)
		{
			return result;
		}
		#endregion
		//	Handles both new and existing invoice
		//	For a new invoice, the following information is needed
		//		Customer & Employee IDs
		//	For a existing invoice, the following information is needed
		//	Invoice & employeeID (We maybe updating an invoice at a later date
		//		and we need the current employee who is handling the transaction

		InvoiceView invoice = null;
		//	new invoice for a customer
		if (invoiceID == 0)
		{
			invoice = new InvoiceView()
			{
				CustomerID = customerID,
				EmployeeID = employeeID,
				InvoiceDate = DateOnly.FromDateTime(DateTime.Now)
			};
		}
		else
		{
			invoice = _hogWildContext.Invoices
						.Where(x => x.InvoiceID == invoiceID
									&& !x.RemoveFromViewFlag)
						.Select(x => new InvoiceView
						{
							InvoiceID = x.InvoiceID,
							InvoiceDate = x.InvoiceDate,
							CustomerID = x.CustomerID,
							EmployeeID = x.EmployeeID,
							SubTotal = x.SubTotal,
							Tax = x.Tax,
							RemoveFromViewFlag = x.RemoveFromViewFlag, //  this will always be false
							InvoiceLines = x.InvoiceLines
											.Select(il => new InvoiceLineView
											{
												InvoiceLineID = il.InvoiceLineID,
												InvoiceID = il.InvoiceID,
												PartID = il.PartID,
												Quantity = il.Quantity,
												Description = il.Part.Description,
												Price = il.Part.Price,
												Taxable = (bool)il.Part.Taxable,
												RemoveFromViewFlag = il.RemoveFromViewFlag
											}).ToList()
						}).FirstOrDefault();
			customerID = invoice.CustomerID;
		}
		invoice.CustomerName = GetCustomerFullName(customerID);
		invoice.EmployeeName = GetEmployeeFullName(invoice.EmployeeID);

		//	only happen if the invoice was mark as remove
		if (invoice == null)
		{
			//	need to exit because we did not find any invoice
			return result.AddError(new Error("No Invoice", "No invoice was found"));
		}
		return result.WithValue(invoice);

	}

	//	Get parts
	public Result<List<PartView>> GetParts(int partCategoryID, string description, List<int> existingPartIDs)
	{
		//	Create a Result container that will hold either a 
		//		list of PartView on success or any accumulated errors on failure
		var result = new Result<List<PartView>>();

		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data
		//		rule:	Both part id must be valid and/or description cannot be empty
		//		rule: 	Part IDs in existing part IDs will be ignored
		//		rule:	RemoveFromViewFlag must be false


		//	Both part id must be valid and/or description cannot be empty
		if (partCategoryID == 0 && string.IsNullOrWhiteSpace(description))
		{
			return result.AddError(new Error("Missing Information",
								"Please provide either a category and/or description"));
		}
		#endregion

		// Start with the base query, filtering out:
		// - any parts already in the "existing part IDs" list
		// - any parts flagged for removal from view
		var query = _hogWildContext.Parts
			.Where(p => !existingPartIDs.Contains(p.PartID) && !p.RemoveFromViewFlag);

		// Determine which search criteria the user actually provided.
		// A description is considered "provided" only if it's not null, empty, or whitespace.
		// A category is considered "provided" only if the ID is greater than zero.
		bool hasDescription = !string.IsNullOrWhiteSpace(description);
		bool hasCategory = partCategoryID > 0;

		if (hasDescription && hasCategory)
		{
			// Both search criteria were provided:
			// Filter parts where the description contains the search text (case-insensitive)
			// AND the part belongs to the specified category.
			// Using AND because we want to narrow results when both filters are active.
			query = query.Where(p =>
				p.Description.ToUpper().Contains(description.ToUpper())
				&& p.PartCategoryID == partCategoryID);
		}
		else if (hasDescription)
		{
			// Only a description was provided (no category selected):
			// Filter parts where the description contains the search text (case-insensitive).
			query = query.Where(p =>
				p.Description.ToUpper().Contains(description.ToUpper()));
		}
		else if (hasCategory)
		{
			// Only a category was provided (no description entered):
			// Filter parts that belong to the specified category.
			query = query.Where(p =>
				p.PartCategoryID == partCategoryID);
		}
		// If neither criteria was provided, no additional filtering is applied,
		// and the base query (excluding existing parts and removed parts) is used as-is.

		// Project the filtered results into PartView objects,
		// selecting only the fields needed for the view.
		// Then sort alphabetically by description.
		// Finally, execute the query and materialize the results into a list.
		var parts = query
			.Select(p => new PartView
			{
				PartID = p.PartID,
				PartCategoryID = p.PartCategoryID,
				CategoryName = p.PartCategory.Name,  // Navigation property to get the category name
				Description = p.Description,
				Cost = p.Cost,
				Price = p.Price,
				ROL = p.ROL,                         // Reorder Level
				QOH = p.QOH,                         // Quantity On Hand
				Taxable = p.Taxable,
				RemoveFromViewFlag = p.RemoveFromViewFlag
			})
			.OrderBy(p => p.Description)    // Sort results alphabetically by description
			.ToList();                       // Execute the query against the database

		//  if no parts were found
		if (parts == null || parts.Count() == 0)
		{
			//need to exit because we did not find any parts
			return result.AddError(new Error("No Parts", "No parts were found"));
		}

		//	return the result
		return result.WithValue(parts);
	}

	//	Get the part
	public Result<PartView> GetPart(int partID)
	{
		// Create a Result container that will hold either a
		//	PartView objects on success or any accumulated errors on failure
		var result = new Result<PartView>();
		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		rule:	partID must be valid
		//		rule: 	RemoveFromViewFlag must be false
		if (partID == 0)
		{
			//  need to exit because we have no part information
			return result.AddError(new Error("Missing Information",
							"Please provide a valid part id"));
		}
		#endregion

		var part = _hogWildContext.Parts
						.Where(p => (p.PartID == partID
									 && !p.RemoveFromViewFlag))
						.Select(p => new PartView
						{
							PartID = p.PartID,
							PartCategoryID = p.PartCategoryID,
							//  PartCategory is an alias for Lookup
							CategoryName = p.PartCategory.Name,
							Description = p.Description,
							Cost = p.Cost,
							Price = p.Price,
							ROL = p.ROL,
							QOH = p.QOH,
							Taxable = (bool)p.Taxable,
							RemoveFromViewFlag = p.RemoveFromViewFlag
						}).FirstOrDefault();

		//  if no part were found
		if (part == null)
		{
			//  need to exit because we did not find any part
			return result.AddError(new Error("No part", "No part were found"));
		}

		//  return the result
		return result.WithValue(part);
	}

	//	add edit invoice
	public Result<InvoiceView> AddEditInvoice(InvoiceView invoiceView)
	{
		//	Create a result container that will hold either a 
		//		invoice view object on sucess or any accumulated errors on failure
		var result = new Result<InvoiceView>();

		#region Business Rules
		//	These are processing rules that need to be satisfied
		//		for valid data
		//		rule:	invoice view cannot be null
		if (invoiceView == null)
		{
			//	need to exit beccause we have no invoice object
			return result.AddError(new Error("Missing Invoice", "No invoice was supply"));
		}

		//		rule:  	cusomerID must be provided if invoiceID == 0
		if (invoiceView.CustomerID == 0 && invoiceView.InvoiceID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a valid customer ID"));
		}

		//		Rule:  	employeeID must be provided
		if (invoiceView.EmployeeID == 0)
		{
			result.AddError(new Error("Missing Information", "Please provide a valid employee ID"));
		}

		//		rule: there must be incoice lines provided
		//				make sure that your InvoiceLines have been initialize (xxx = new List<InvoiceLineView>();
		if (invoiceView.InvoiceLines.Count() == 0)
		{
			return result.AddError(new Error("Missing Information", "Invoice details are required"));
		}

		//		rule:	for each invoice line, there must be a part id
		//		rule:	for each invoice line, price cannot be less than zero
		//		rule:	for each invoice line, quqntity cannot be less than 1
		foreach (var invoiceLine in invoiceView.InvoiceLines)
		{
			if (invoiceLine.PartID == 0)
			{
				//	need to exit because we have no part information to process
				return result.AddError(new Error("Missing Information", "Missting Part ID"));
			}

			if (invoiceLine.Price < 0)
			{
				string partName = _hogWildContext.Parts
									.Where(p => p.PartID == invoiceLine.PartID)
									.Select(p => p.Description).FirstOrDefault();
				result.AddError(new Error("Invalid Price", $"Part {partName} has a price less than zero"));
			}


			if (invoiceLine.Quantity < 1)
			{
				string partName = _hogWildContext.Parts
									.Where(p => p.PartID == invoiceLine.PartID)
									.Select(p => p.Description).FirstOrDefault();
				result.AddError(new Error("Invalid Quantity", $"Part {partName} has a quantity less than one"));
			}

		}

		//	rule:	parts cannot be duplicated on more than one line.
		List<string> duplicatedParts = invoiceView.InvoiceLines
										.GroupBy(i => new { i.PartID })
										.Where(g => g.Count() > 1)
										.OrderBy(g => g.Key.PartID)
										.Select(g => _hogWildContext.Parts
													.Where(p => p.PartID == g.Key.PartID)
													.Select(p => p.Description)
													.FirstOrDefault()
										).ToList();

		if (duplicatedParts.Count() > 0)
		{
			foreach (var partName in duplicatedParts)
			{
				result.AddError(new Error("Duplicated Invoice Line Items",
										$"Part {partName} can only be added to the invoice lines once"));
			}
		}

		// need to exit if there are any outstanding issues
		if (result.IsFailure)
		{
			return result;
		}
		#endregion

		//	retrieve the invoice from the database or create a new record/entity if it does not exist
		Invoice invoice = _hogWildContext.Invoices
							.Where(i => i.InvoiceID == invoiceView.InvoiceID
										&& i.RemoveFromViewFlag == false) // !i.RemoveFromViewFlag
							.Select(i => i).FirstOrDefault();

		//	if the invoice doesn't exist, initizlize it
		if (invoice == null)
		{
			invoice = new Invoice();
			//	set the current date for the new invoice
			invoice.InvoiceDate = DateOnly.FromDateTime(DateTime.Now);
			invoice.CustomerID = invoiceView.CustomerID;
		}
		
		//	update invoice properties (fields) from the view model
		invoice.EmployeeID = invoiceView.EmployeeID;
		invoice.RemoveFromViewFlag = invoiceView.RemoveFromViewFlag;
		
		//	reset the subotal & tax as this will be updated from the invoice lines
		invoice.SubTotal = 0;
		invoice.Tax = 0;
		
		//	process each line item in the proived invoice lines view model
		foreach(var invoiceLineView in invoiceView.InvoiceLines)
		{
			//	get the record/entity
			InvoiceLine invoiceLine = _hogWildContext.InvoiceLines
										.Where(il => il.InvoiceLineID == invoiceLineView.InvoiceLineID
													&& !il.RemoveFromViewFlag)
										.Select(il => il).FirstOrDefault();
			
			//	if the line item does not exist, initialize it
			if(invoiceLine == null)
			{
				invoiceLine = new InvoiceLine();
				invoiceLine.PartID = invoiceLineView.PartID;
			}
										
			//	update invoice line properties (fields) from the view model	
			invoiceLine.Quantity = invoiceLineView.Quantity;
			invoiceLine.Price = invoiceLineView.Price;
			invoiceLine.RemoveFromViewFlag = invoiceLineView.RemoveFromViewFlag;
			
			//	handle a new or existing line item
			if(invoiceLine.InvoiceLineID == 0)
			{
				//	add new line item to the invoice entity/record
				invoice.InvoiceLines.Add(invoiceLine);
			}
			else
			{
				//	update the database record with the existing line item
				_hogWildContext.InvoiceLines.Update(invoiceLine);
			}
			
			//	need to update subtotal and tax if the invoice line item is 
			//		not set to be removed from view
			if(invoiceLine.RemoveFromViewFlag == false)
			{
				invoice.SubTotal += invoiceLine.Quantity * invoiceLine.Price;
				bool isTaxable = _hogWildContext.Parts
									.Where(p => p.PartID == invoiceLine.PartID)
									.Select(p => p.Taxable).FirstOrDefault();
									
				invoice.Tax += isTaxable ? invoiceLine.Quantity * invoiceLine.Price * .05m : 0;
			}
		}

		//	if it is a new invoice, add it to the collection
		if(invoice.InvoiceID == 0)
		{
			//	add the invoice to the invoice table
			_hogWildContext.Invoices.Add(invoice);
		}
		else
		{
			//	update the invoice in the invoice table
			_hogWildContext.Invoices.Update(invoice);
		}

		try
		{
			//	NOTE: YOU CAN ONLY HAVE ONE SAVE CHANGES IN A METHOD
			_hogWildContext.SaveChanges();
		}
		catch (Exception ex)
		{
			//	clear change to maintain data integrity
			_hogWildContext.ChangeTracker.Clear();
			//	we  do not have to throw an exception, just need to log the erro message
			return result.AddError(new Error("Error Saving Changes", ex.InnerException.Message));
		}
		
		return GetInvoice(invoice.InvoiceID, invoice.CustomerID, invoice.EmployeeID);


	}

	//	get the customer full name
	public string GetCustomerFullName(int customerID)
	{
		return _hogWildContext.Customers
					.Where(c => c.CustomerID == customerID)
					.Select(c => $"{c.FirstName} {c.LastName}").FirstOrDefault() ?? string.Empty;
	}

	//	get the employee full name
	public string GetEmployeeFullName(int employeeID)
	{
		return _hogWildContext.Employees
					.Where(e => e.EmployeeID == employeeID)
					.Select(e => $"{e.FirstName} {e.LastName}").FirstOrDefault() ?? string.Empty;
	}
}
#endregion

// ———— PART 4: View Models → Service Library View Model ————
//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class PartView
{
	public int PartID { get; set; }
	public int PartCategoryID { get; set; }
	public string CategoryName { get; set; }
	public string Description { get; set; }
	public decimal Cost { get; set; }
	public decimal Price { get; set; }
	public int ROL { get; set; }
	public int QOH { get; set; }
	public bool Taxable { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}

public class InvoiceView
{
	public int InvoiceID { get; set; }
	public DateOnly InvoiceDate { get; set; }
	public int CustomerID { get; set; }
	public string CustomerName { get; set; }
	public int EmployeeID { get; set; }
	public string EmployeeName { get; set; }
	public decimal SubTotal { get; set; }
	public decimal Tax { get; set; }
	public decimal Total => SubTotal + Tax;
	public List<InvoiceLineView> InvoiceLines { get; set; } = new List<InvoiceLineView>();
	public bool RemoveFromViewFlag { get; set; }
}

public class InvoiceLineView
{
	public int InvoiceLineID { get; set; }
	public int InvoiceID { get; set; }
	public int PartID { get; set; }
	public string Description { get; set; }
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public bool Taxable { get; set; }
	public decimal ExtentPrice => Price * Quantity;
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