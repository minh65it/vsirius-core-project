using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StullerApiClientDomain.Invoices
{
    public class InvoiceRequest
    {
        public List<string> PurchaseOrderNumber { get; set; }
        public List<string> InvoiceNumber { get; set; }
        public List<Filters> Filters { get; set; }
    }
    public class FilterValue
    {
        public string DisplayValue { get; set; }
        public string Value { get; set; }
    }
    public class Filters
    {
        public string Filter { get; set; }
        public List<FilterValue> Values { get; set; }
    }
}
