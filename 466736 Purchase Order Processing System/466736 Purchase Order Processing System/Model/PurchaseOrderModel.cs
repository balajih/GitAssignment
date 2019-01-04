using System;

namespace _466736_Purchase_Order_Processing_System.Model
{
    public class PurchaseOrderModel
    {
        public string PONumber { get; set; }
        public DateTime Date { get; set; }
        public string SupplierNumber { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
    }
}