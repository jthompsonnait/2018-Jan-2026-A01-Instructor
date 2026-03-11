using HogWildSystem.BLL;
using HogWildSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem
{
    public static class HogWildExtension
    {
        // This is an extension method that extends the IServiceCollection interface.
        // It is typically used in ASP.NET Core applications to configure and register services.

        // The method name can be anything, but it must match the name used when calling it in
        // your Program.cs file using builder.Services.XXXX(options => ...).
        //  Rename this to your module ieL  AddPODependencies(), AddReceivingDependencies
        public static void AddBackendDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            // Register the HogWildContext class, which is the DbContext for your application,
            // with the service collection. This allows the DbContext to be injected into other
            // parts of your application as a dependency.

            // The 'options' parameter is an Action<DbContextOptionsBuilder> that typically
            // configures the options for the DbContext, including specifying the database
            // connection string.
            services.AddDbContext<HogWildContext>(options);

            //  adding any services that you create in the class library (bll)
            //  using .AddTransient<t>(...)
            //  working version
            services.AddTransient<WorkingVersionService>((ServiceProvider) =>
            {
                //  Retrieve an instance of HogWIldContext from the service provider
                var context = ServiceProvider.GetService<HogWildContext>();

                //  Create a new instance of WorkingVersionService
                //      passing the HogWildContext instance as a parameter
                return new WorkingVersionService(context);

            });

            //  customer service
            services.AddTransient<CustomerService>((ServiceProvider) =>
            {
                //  Retrieve an instance of HogWIldContext from the service provider
                var context = ServiceProvider.GetService<HogWildContext>();

                //  Create a new instance of CustomerService
                //      passing the HogWildContext instance as a parameter
                return new CustomerService(context);

            });

            //  category/lookup service
            services.AddTransient<CategoryLookupService>((ServiceProvider) =>
            {
                //  Retrieve an instance of HogWIldContext from the service provider
                var context = ServiceProvider.GetService<HogWildContext>();

                //  Create a new instance of CategoryLookupService
                //      passing the HogWildContext instance as a parameter
                return new CategoryLookupService(context);

            });

            //  part service
            services.AddTransient<PartService>((ServiceProvider) =>
            {
                //  Retrieve an instance of HogWIldContext from the service provider
                var context = ServiceProvider.GetService<HogWildContext>();

                //  Create a new instance of PartService
                //      passing the HogWildContext instance as a parameter
                return new PartService(context);

            });
            //  invoice service
            services.AddTransient<InvoiceService>((ServiceProvider) =>
            {
                //  Retrieve an instance of HogWIldContext from the service provider
                var context = ServiceProvider.GetService<HogWildContext>();

                //  Create a new instance of InvoiceService
                //      passing the HogWildContext instance as a parameter
                return new InvoiceService(context);

            });
        }
    }
}
