using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ReceiptDetailModel : BaseEntity
    {
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountUnitPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public ReceiptDetailModel(ReceiptDetail receipt)
        {
            if (receipt == null)
            {
                return;
            }
            Id = receipt.Id;
            ReceiptId = receipt.ReceiptId;
            ProductId = receipt.ProductId;
            DiscountUnitPrice = receipt.DiscountUnitPrice;
            UnitPrice = receipt.UnitPrice;
            Quantity = receipt.Quantity;
        }

        public ReceiptDetailModel()
        {
        }
    }
}
