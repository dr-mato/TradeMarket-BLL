using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CustomerModel : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int DiscountValue { get; set;}
        public ICollection<int> ReceiptsIds { get; set; }

        public CustomerModel(Customer customer)
        {
            if (customer == null)
            {
                return;
            }
            Id = customer.Id;
            Name = customer.Person?.Name ?? string.Empty;
            Surname = customer.Person?.Surname ?? string.Empty;
            BirthDate = customer.Person?.BirthDate ?? DateTime.MinValue;
            DiscountValue = customer.DiscountValue;
            ReceiptsIds = customer.Receipts?.Select(r => r.Id).ToList() ?? new List<int>();
        }

        public CustomerModel()
        {
        }
    }
}
