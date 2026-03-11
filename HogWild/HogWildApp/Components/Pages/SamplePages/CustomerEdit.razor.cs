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
        //  the customer
        private CustomerEditView customer = new();

        //  mudform control
        private MudForm customerForm = new MudForm();
        #endregion

        #region Feedback & Error Messages
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
        //  customer service
        [Inject]
        protected CustomerService? CustomerService { get; set; } = null;

        [Parameter]
        public int CustomerID { get; set; } = 0;
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
                //  check to see if we have an existing customer
                if (CustomerID > 0)
                {
                    var result = CustomerService.GetCustomer(CustomerID);
                    if (result.IsSuccess)
                    {
                        customer = result.Value;
                    }
                    else
                    {
                        errorDetails = BlazorHelperClass.GetErrorMessages(result.Errors.ToList());
                    }
                }
                else
                {
                    customer = new CustomerEditView();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                //	capture any exception message for display
                errorMessage = ex.Message;
            }



            #endregion

        }
    }
}
