using _466736_Purchase_Order_Processing_System.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace _466736_Purchase_Order_Processing_System
{
    [ServiceContract]
    public interface IPurchaseOrder
    {
        [OperationContract]
        string CreateSupplier(SupplierModel supplier);
        [OperationContract]
        void UpdateSupplier(SupplierModel supplier);
        [OperationContract]
        void DeleteSupplier(string id);
        [OperationContract]
        List<SupplierModel> GetSuppliers();
        [OperationContract]
        SupplierModel GetSupplier(string id);
        //////////
        [OperationContract]
        string CreateItem(ItemModel item);
        [OperationContract]
        void UpdateItem(ItemModel item);
        [OperationContract]
        void DeleteItem(string code);
        [OperationContract]
        List<ItemModel> GetItems();
        [OperationContract]
        ItemModel GetItem(string code);
        //////////
        [OperationContract]
        void CreatePurchaseOrder(PurchaseOrderModel POModel);
        [OperationContract]
        void UpdatePurchaseOrder(PurchaseOrderModel POModel);
        [OperationContract]
        void DeletePurchaseOrder(string orderNumber);
        [OperationContract]
        List<PurchaseOrderModel> GetPurchaseOrders();
        [OperationContract]
        PurchaseOrderModel GetPurchaseOrder(string code);
    }
}
