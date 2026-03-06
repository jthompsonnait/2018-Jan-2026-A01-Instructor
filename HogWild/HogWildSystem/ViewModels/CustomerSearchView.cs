using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.ViewModels
{
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
        public decimal? TotalSales { get; set; } = 0;
    }
}
