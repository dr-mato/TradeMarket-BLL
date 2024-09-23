using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ReceiptModel : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }
        public ICollection<int> ReceiptDetailsIds { get; set; }

        public ReceiptModel(Receipt receipt)
        {
            if (receipt == null)
            {
                return;
            }
            Id = receipt.Id;
            CustomerId = receipt.CustomerId;
            OperationDate = receipt.OperationDate;
            IsCheckedOut = receipt.IsCheckedOut;
            ReceiptDetailsIds = receipt.ReceiptDetails.Select(rd => rd.Id).ToList();
        }

        public ReceiptModel()
        {
        }
    }
}
