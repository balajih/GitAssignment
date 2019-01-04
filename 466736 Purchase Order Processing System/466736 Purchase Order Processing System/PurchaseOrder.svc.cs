using _466736_Purchase_Order_Processing_System.DataObjects;
using _466736_Purchase_Order_Processing_System.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace _466736_Purchase_Order_Processing_System
{
    /// <summary>
    /// Purchase Order CRUDs
    /// </summary>
    public class PurchaseOrder : IPurchaseOrder
    {
        private PODbEntities dbContext = new PODbEntities();
        /// <summary>
        /// Add new supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public string CreateSupplier(SupplierModel supplier)
        {
            var dbSupplier = new SUPPLIER
            {
                SUPLADDR = supplier.Address,
                SUPLNAME = supplier.Name,
                SUPLNO = supplier.Id
            };
            dbContext.SUPPLIER.Add(dbSupplier);
            dbContext.SaveChanges();
            return dbSupplier.SUPLNO;
        }

        public void UpdateSupplier(SupplierModel supplier)
        {
            var dbSupplier = new SUPPLIER
            {
                SUPLADDR = supplier.Address,
                SUPLNAME = supplier.Name,
                SUPLNO = supplier.Id
            };
            dbContext.Entry(dbSupplier).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void DeleteSupplier(string id)
        {
            var supplier = dbContext.SUPPLIER.FirstOrDefault(x => x.SUPLNO == id);
            if (supplier != null)
            {
                dbContext.SUPPLIER.Remove(supplier);
                dbContext.SaveChanges();
            }
        }

        public List<SupplierModel> GetSuppliers()
        {
            return dbContext.SUPPLIER.Select(x => new SupplierModel
            {
                Address = x.SUPLADDR,
                Id = x.SUPLNO,
                Name = x.SUPLNAME
            }).ToList();
        }

        public SupplierModel GetSupplier(string id)
        {
            var x = dbContext.SUPPLIER.FirstOrDefault(y => y.SUPLNO == id);
            return x != null ? new SupplierModel
            {
                Address = x.SUPLADDR,
                Id = x.SUPLNO,
                Name = x.SUPLNAME
            } : null;
        }

        ///////////////

        public string CreateItem(ItemModel item)
        {
            var dbItem = new ITEM
            {
                ITCODE = item.Code,
                ITDESC = item.Desc,
                ITRATE = item.Rate
            };
            dbContext.ITEM.Add(dbItem);
            dbContext.SaveChanges();
            return dbItem.ITCODE;
        }

        public void UpdateItem(ItemModel item)
        {
            var dbItem = new ITEM
            {
                ITCODE = item.Code,
                ITDESC = item.Desc,
                ITRATE = item.Rate
            };
            dbContext.Entry(dbItem).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void DeleteItem(string code)
        {
            var item = dbContext.ITEM.FirstOrDefault(x => x.ITCODE == code);
            if (item != null)
            {
                dbContext.ITEM.Remove(item);
                dbContext.SaveChanges();
            }
        }

        public List<ItemModel> GetItems()
        {
            return dbContext.ITEM.Select(x => new ItemModel
            {
                Code = x.ITCODE,
                Desc = x.ITDESC,
                Rate = x.ITRATE.Value
            }).ToList();
        }

        public ItemModel GetItem(string code)
        {
            var x = dbContext.ITEM.FirstOrDefault(y => y.ITCODE == code);
            return x != null ? new ItemModel
            {
                Code = x.ITCODE,
                Desc = x.ITDESC,
                Rate = x.ITRATE.Value
            } : null;
        }

        ///////////////

        public void CreatePurchaseOrder(PurchaseOrderModel POModel)
        {
            var dbMaster = new POMASTER
            {
                PONO = POModel.PONumber,
                PODATE = POModel.Date,
                SUPLNO = POModel.SupplierNumber
            };

            var dbDetail = new PODETAIL
            {
                PONO = POModel.PONumber,
                ITCODE = POModel.ItemCode,
                QTY = POModel.Quantity
            };

            dbContext.POMASTER.Add(dbMaster);
            dbContext.PODETAIL.Add(dbDetail);
            dbContext.SaveChanges();
        }

        public void UpdatePurchaseOrder(PurchaseOrderModel POModel)
        {
            var dbMaster = new POMASTER
            {
                PONO = POModel.PONumber,
                PODATE = POModel.Date,
                SUPLNO = POModel.SupplierNumber
            };

            var dbDetail = new PODETAIL
            {
                PONO = POModel.PONumber,
                ITCODE = POModel.ItemCode,
                QTY = POModel.Quantity
            };

            dbContext.Entry(dbMaster).State = EntityState.Modified;
            dbContext.Entry(dbDetail).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void DeletePurchaseOrder(string orderNumber)
        {
            var dbMaster = dbContext.POMASTER.FirstOrDefault(x => x.PONO == orderNumber);
            var dbDetail = dbContext.PODETAIL.FirstOrDefault(x => x.PONO == orderNumber);
            if (dbMaster != null)
            {
                dbContext.POMASTER.Remove(dbMaster);
                dbContext.SaveChanges();
            }
            if (dbDetail != null)
            {
                dbContext.PODETAIL.Remove(dbDetail);
                dbContext.SaveChanges();
            }
        }

        public List<PurchaseOrderModel> GetPurchaseOrders()
        {
            return dbContext.PODETAIL.Select(x => new PurchaseOrderModel
            {
                PONumber = x.PONO,
                Date = x.POMASTER.PODATE.Value,
                ItemCode = x.ITCODE,
                Quantity = x.QTY.Value,
                SupplierNumber = x.POMASTER.SUPLNO
            }).ToList();
        }

        public PurchaseOrderModel GetPurchaseOrder(string code)
        {
            var x = dbContext.PODETAIL.Include(y => y.POMASTER).FirstOrDefault(y => y.PONO == code);
            return x != null ? new PurchaseOrderModel
            {
                PONumber = x.PONO,
                Date = x.POMASTER.PODATE.Value,
                ItemCode = x.ITCODE,
                Quantity = x.QTY.Value,
                SupplierNumber = x.POMASTER.SUPLNO
            } : null;
        }
    }
}
