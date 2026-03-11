using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogWildSystem.ViewModels
{
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
}
