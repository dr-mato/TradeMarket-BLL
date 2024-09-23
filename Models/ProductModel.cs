using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ProductModel : BaseEntity
    {
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public ICollection<int> ReceiptDetailIds { get; set; }
        
        public ProductModel(Product product)
        {
            if (product == null)
            {
                return;
            }
            Id = product.Id;
            ProductCategoryId = product.ProductCategoryId;
            CategoryName = product.Category?.CategoryName ?? string.Empty;
            ProductName = product.ProductName;
            Price = product.Price;
            ReceiptDetailIds = product.ReceiptDetails?.Select(r => r.Id).ToList() ?? new List<int>();
        }

        public ProductModel()
        {
        }
    }
}
