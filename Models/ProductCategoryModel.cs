using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ProductCategoryModel : BaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<int> ProductIds { get; set; }

        public ProductCategoryModel(ProductCategory category)
        {
            if (category == null)
            {
                return;
            }
            CategoryName = category.CategoryName;
            ProductIds = category.Products?.Select(p => p.Id).ToList() ?? new List<int>();
        }

        public ProductCategoryModel()
        {
        }
    }
}
