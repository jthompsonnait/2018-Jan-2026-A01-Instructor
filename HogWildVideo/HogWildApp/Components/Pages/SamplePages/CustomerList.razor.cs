using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerList
    {
        #region Fields
        //  last name
        private string lastName = string.Empty;
        // phone number
        private string phone = string.Empty;
        // tell us if the search has been performed
        private bool noRecords;
        //  feedback message
        private string feedbackMessage = string.Empty;
        // error message
        private string errorMessage = string.Empty;
        //  has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);
        //  has error
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage) || errorDetails.Count > 0;
        //  error details
        private List<string> errorDetails = new();
        #endregion

        #region Properties
        //  Injecting the CustomerService dependency
        [Inject]
        protected CustomerService? CustomerService { get; set; } = null;

        //  Inject the NavigationManager dependency
        [Inject] protected NavigationManager? NavigationManager { get; set; } = null;

        //  Get or set the customer search views
        protected List<CustomerSearchView> Customers { get; set; } = new();

        #endregion

        #region Methods
        //  search for existing customers
        private void Search()
        {
            //	clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;
            noRecords = false;
            Customers.Clear();

            //	wrap the service call in a try/catch to handle unexpected exception
            try
            {
                var result = CustomerService.GetCustomers(lastName, phone);
                if (result.IsSuccess)
                {
                    Customers = result.Value;
                }
                else
                {
                    //  set noRecord
                    //  could be written cleaner
                    //  noRecords = result.Errors.Any(e => e.Code == "No Customer");
                    if (result.Errors.Any(e => e.Code == "No Customer"))
                    {
                        noRecords = true;
                    }

                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        //  new customer
        private void New()
        {
            NavigationManager.NavigateTo("/SamplePages/CustomerEdit/0");
        }

        //  edit a selected customer
        private void EditCustomer(int customerID)
        {
            NavigationManager.NavigateTo($"/SamplePages/CustomerEdit/{customerID}");
        }

        //  new invoice for selected customer
        private void NewInvoice(int customerID)
        {

        }
        #endregion

    }
}
