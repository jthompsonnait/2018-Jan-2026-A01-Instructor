#nullable disable
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildApp.Components.Pages.SamplePages
{
    public partial class WorkingVersion
    {
        #region Fields
        //  Property for holding any feedback message
        private string feedback;

        //  This private field holds a reference to the WorkingVersionView instance
        private WorkingVersionView workingVersionView = new();
        #endregion

        #region Properties
        //  This attribute marks the property for dependecy injections
        [Inject]
        //  This property provides access to the "WorkingVersionService" service
        protected WorkingVersionService WorkingVersionService { get; set; }
        #endregion

        #region Methods
        private void GetWorkingVersion()
        {
            try
            {
                workingVersionView = WorkingVersionService.GetWorkingVersion();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
