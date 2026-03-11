using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.IO;
using static MudBlazor.Icons;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class InvoiceEdit
    {
        #region Fields
        //  description is for the part search
        private string description = string.Empty;

        //  category id
        private int? categoryID;

        //  part categories
        private List<LookupView> partCategories;

        //  parts
        private List<PartView> parts = new List<PartView>();

        //  invoice
        private InvoiceView invoice = new InvoiceView();

        // no parts
        private bool noParts = false;
        #endregion

        #region Feedback  & Error Messages
        // feedback message
        private string feedbackMessage = string.Empty;
        //  error message
        private string? errorMessage;

        // has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        // has errors
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count > 0;

        //  display any collection of errors on our web page
        //  whether the errors are generated locally or come from the class library
        private List<string> errorDetails = new();
        #endregion

        #region Properties
        //  Inject the NavigationManager dependency
        [Inject] protected NavigationManager? NavigationManager { get; set; } = null;

        //  the invoice service
        [Inject] protected InvoiceService? InvoiceService { get; set; } = null;

        //  part service
        [Inject]
        protected PartService? PartService { get; set; } = null;

        //  category/lookup service
        [Inject]
        protected CategoryLookupService? CategoryLookupService { get; set; } = null;

        //  Inject a DialogService dependency
        [Inject] protected IDialogService? DialogService { get; set; } = null;

        [Parameter] public int InvoiceID { get; set; } = 0;
        [Parameter] public int CustomerID { get; set; } = 0;

        [Parameter] public int EmployeeID { get; set; } = 0;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //	clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //	wrap the service call in a try/catch to handle unexpected exception
            try
            {
                var invoiceResult = InvoiceService.GetInvoice(InvoiceID, CustomerID, EmployeeID);
                if (invoiceResult.IsSuccess)
                {
                    invoice = invoiceResult.Value;
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(invoiceResult.Errors.ToList());
                }

                // lookups
                partCategories = CategoryLookupService.GetLookupView("Part Categories").Value;
                //  update that data has changed
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        //  search
        private void SearchParts()
        {
            //	clear previous error details and message
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //  clear the part list before we do our search
            parts.Clear();

            //  reset no parts to false
            noParts = false;

            if (categoryID == null && string.IsNullOrWhiteSpace(description))
            {
                errorMessage = "Please provide either a category and/or description";
                return;
            }

            //  search for parts in our invoice lines
            List<int> existingPartIDs =
                invoice.InvoiceLines
                    .Select(x => x.PartID)
                    .ToList();

            //wrap the service cal in a try/catch to handle unexpected exceptions
            try
            {
                var result = PartService.GetParts(categoryID, description, existingPartIDs);
                if (result.IsSuccess)
                {
                    parts = result.Value;
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                    feedbackMessage = "Search for part(s) was successful";
                }
            }
            catch (Exception ex)
            {
                // capture any exceptions message for display
                errorMessage = ex.Message;
            }
        }

        private async Task Save()
        {
            // clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = String.Empty;

            //  use in the feedback message to indicate
            //      if this is a new invoice or an update
            bool isNewInvoice = false;

            // wrap the service call in a try/catch to handle unexpected exceptions
            try
            {
                var result = InvoiceService.AddEditInvoice(invoice);
                if (result.IsSuccess)
                {
                    isNewInvoice = invoice.InvoiceID == 0;
                    invoice = result.Value;
                    InvoiceID = invoice.InvoiceID;
                    feedbackMessage = isNewInvoice
                        ? $"New INvoice No {invoice.InvoiceID} was created"
                        : $"Invoice No {InvoiceID} was updated";
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        private async Task Close()
        {
            bool? result = await DialogService.ShowMessageBoxAsync("Confirm Cancel",
                "Do you wish to close the invoice editor?  All unsaved changes will be lost.",
                yesText: "Yes", cancelText: "No");

            //  true means affirmative action (e.g. "Yes")
            //  null means the user dismissed the dialog (e.g. clicking "No" or closing the dialog)
            if (result == true)
            {
                NavigationManager.NavigateTo("/SamplePages/CustomerList");
            }
        }

        //  add part
        private async Task AddPart(int partID)
        {
            // clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            try
            {
                var result = PartService.GetPart(partID);
                if (result.IsSuccess)
                {
                    PartView part = result.Value;
                    InvoiceLineView invoiceLine = new InvoiceLineView();
                    invoiceLine.PartID = partID;
                    invoiceLine.Description = part.Description;
                    invoiceLine.Price = part.Price;
                    invoiceLine.Taxable = part.Taxable;
                    invoiceLine.Quantity = 0;
                    invoice.InvoiceLines.Add(invoiceLine);
                    UpdateSubtotalAndTax();

                    //  remove the current part from the part list
                    parts.Remove(parts.Where(p => p.PartID == partID)
                        .FirstOrDefault());
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                }
           
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private void UpdateSubtotalAndTax()
        {
            invoice.SubTotal = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Quantity * x.Price);

            invoice.Tax = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Taxable ? x.Quantity * x.Price * 0.05m : 0);
        }

        private async Task DeleteInvoiceLine(InvoiceLineView invoiceLine)
        {
            bool? result = await DialogService.ShowMessageBoxAsync("Confirm Delete",
                $"Are you sure that you wish to remove {invoiceLine.Description}?",
                yesText: "Remove", cancelText: "Cancel");

            //  true means affirmative action (e.g. "Yes")
            //  null means the user dismissed the dialog (e.g. clicking "No" or closing the dialog)
            if (result == true)
            {
                invoice.InvoiceLines.Remove(invoiceLine);
                UpdateSubtotalAndTax();
            }
        }

        private void QuantityEdited(InvoiceLineView invoiceLine, int newQuantity)
        {
            invoiceLine.Quantity = newQuantity;
            UpdateSubtotalAndTax();
        }

        private void PriceEdited(InvoiceLineView invoiceLine, decimal newPrice)
        {
            invoiceLine.Price = newPrice;
            UpdateSubtotalAndTax();
        }

        //  synchronizes the price from the database
        private void SyncPrice(InvoiceLineView invoiceLine)
        {
            //  find the original price of the part from the database
            try
            {
                var result = PartService.GetPart(invoiceLine.PartID);
                if (result.IsSuccess)
                {
                    PartView part = result.Value;
                    invoiceLine.Price = part.Price;
                    UpdateSubtotalAndTax();
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                // capture any exceptions message for display
                errorMessage = ex.Message;
            }
        }

        #endregion
    }
}
