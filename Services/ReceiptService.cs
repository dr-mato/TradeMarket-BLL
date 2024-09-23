using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : CrudService<ReceiptModel>, IReceiptService
    {
        private readonly IMapper _mapper;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailRepository _receiptDetailRepository;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = unitOfWork.ReceiptRepository;
            _receiptDetailRepository = unitOfWork.ReceiptDetailRepository;
        }
        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _receiptRepository.GetByIdAsync(receiptId);

            if (receipt == null)
            {
                return;
            }

            var product = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
            
            if (product != null) 
            {
                product.Quantity += quantity;
            }

            _receiptRepository.Update(receipt);
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            _receiptRepository.Update(receipt);
        }

        public Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        { 
            var receiptDetails = _receiptDetailRepository.GetAllAsync().Result.Where(rd => rd.ReceiptId == receiptId);
            return (Task<IEnumerable<ReceiptDetailModel>>)receiptDetails.Select(rd => new ReceiptDetailModel(rd));
        }

        public Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = _receiptRepository.GetAllAsync().Result.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);
            return (Task<IEnumerable<ReceiptModel>>)receipts.Select(r => new ReceiptModel(r));
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _receiptRepository.GetByIdAsync(receiptId);

            if (receipt == null)
            {
                return;
            }

            var product = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);
            
            if (product != null)
            {
                if (product.Quantity < quantity)
                {
                    return;
                }
                product.Quantity -= quantity;
            }

            _receiptRepository.Update(receipt);
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receiptDetails = await _receiptDetailRepository.GetAllAsync();

            var total = receiptDetails
                .Where(rd => rd.ReceiptId == receiptId)
                .Select(rd => rd.Quantity * rd.DiscountUnitPrice)
                .Sum();

            return total;
        }
    }
}
