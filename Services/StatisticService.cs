using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IMapper _mapper;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailRepository _receiptDetailsRepository;

        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = unitOfWork.ReceiptRepository;
            _receiptDetailsRepository = unitOfWork.ReceiptDetailRepository;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _receiptRepository.GetAllAsync();
            var customerReceipts = receipts.Where(r => r.CustomerId == customerId);
            var receiptDetails = customerReceipts.SelectMany(cr => cr.ReceiptDetails);
            var popularProducts = receiptDetails
                .GroupBy(rd => rd.Product)
                .OrderByDescending(g => g.Sum(rd => rd.Quantity))
                .Take(productCount)
                .Select(g => new ProductModel(g.Key));

            return popularProducts;
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receiptDetails = await _receiptDetailsRepository.GetAllAsync();
            var neededRDs = receiptDetails
                .Where(rd => rd.Product.ProductCategoryId == categoryId && rd.Receipt.OperationDate >= startDate && rd.Receipt.OperationDate <= endDate);
            return neededRDs.Sum(r => r.Quantity * r.DiscountUnitPrice);

        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _receiptDetailsRepository.GetAllAsync();
            var popularProducts = receiptDetails
                .GroupBy(rd => rd.Product)
                .OrderByDescending(g => g.Sum(rd => rd.Quantity))
                .Take(productCount)
                .Select(g => new ProductModel(g.Key));

            return popularProducts;
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receiptDetails = await _receiptDetailsRepository.GetAllAsync();
            return receiptDetails.Where(rd => rd.Receipt.OperationDate >= startDate && rd.Receipt.OperationDate <= endDate)
                .GroupBy(rd => rd.Receipt.Customer)
                .Select(g => new CustomerActivityModel
                {
                    CustomerId = g.Key.Id,
                    CustomerName = g.Key.Person.Name,
                    ReceiptSum = g.Sum(rd => rd.Quantity * rd.DiscountUnitPrice)
                })
                .OrderByDescending(c => c.ReceiptSum)
                .Take(customerCount);
        }
    }
}
