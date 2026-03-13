using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Icons;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class CustomerEdit
    {
        #region Fields
        // customer
        private CustomerEditView customer = new();
        //  provinces
        private List<LookupView> provinces = new();
        //  countries
        private List<LookupView> countries = new();
        //  status
        private List<LookupView> statusLookups = new();



        //  mudform control
        private MudForm customerForm = new();
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
        //  the customer service
        [Inject] protected CustomerService? CustomerService { get; set; } = null;

        //  category/lookup service
        [Inject]
        protected CategoryLookupService? CategoryLookupService { get; set; } = null;

        //  Inject the NavigationManager dependency
        [Inject] protected NavigationManager? NavigationManager { get; set; } = null;

        //  Inject a DialogService dependency
        [Inject] protected IDialogService? DialogService { get; set; } = null;

        [Parameter] public int CustomerID { get; set; } = 0;

        #endregion

        #region Validation
        //  flag to check if the form is valid
        private bool isFormValid = false;
        //  has the form changed (isDirty)
        private bool isDirty = false;
        //  set text for cancel/close button
        private string closeButtonText => isDirty ? "Cancel" : "Close";
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
                //  check to see if we are naviagating using a valid customer CustomerID
                //      or are we going to create a new customer.
                if (CustomerID > 0)
                {
                    var result = CustomerService.GetCustomer(CustomerID);

                    if (result.IsSuccess)
                    {
                        customer = result.Value;
                        feedbackMessage = "Data was successfully saved";

                    }
                    else
                    {
                        errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                    }
                }
                else
                {
                    customer = new();
                }

                // lookups
                provinces = CategoryLookupService.GetLookupView("Province").Value;
                countries = CategoryLookupService.GetLookupView("Country").Value;
                statusLookups = CategoryLookupService.GetLookupView("Customer Status").Value;

                //  update that data has changed
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }
        }

        //  save the customer
        private async Task Save()
        {
            //	clear previous error details and messages
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            //	wrap the service call in a try/catch to handle unexpected exception
            try
            {
                var result = CustomerService.AddEditCustomer(customer);
                if (result.IsSuccess)
                {
                    customer = result.Value;
                    feedbackMessage = "Customer was successfully saved!";

                    //  reset is dirty to false
                    isDirty = false;
                }
                else
                {
                    errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }

            StateHasChanged();
        }

        //  cancel/close of this instance
        private async Task Cancel()
        {
            if (isDirty)
            {
                bool? result = await DialogService.ShowMessageBoxAsync("Confirm Cancel",
                    "Do you wish to close the customer editor?  All unsaved changes will be lost.",
                    yesText: "Yes", cancelText: "No");

                //  true means affirmative action (e.g. "Yes")
                //  null means the user dismissed the dialog (e.g. clicking "No" or closing the dialog)
                if (result == null)
                {
                    return;
                }
            }
            NavigationManager.NavigateTo("/SamplePages/CustomerList");
        }


        private void OnFieldChanged()
        {
            isDirty = true;
        }

        #endregion
    }
}
