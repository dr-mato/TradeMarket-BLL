using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : CrudService<CustomerModel>, ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            if (unitOfWork == null) { return; }
            _customerRepository = unitOfWork.CustomerRepository;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var allCustomers = await _customerRepository.GetAllAsync();
            var customersWithProductIds = allCustomers.AsQueryable()
                .Where(allCustomers => allCustomers.Receipts.Any(r => r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
            return customersWithProductIds.Select(c => new CustomerModel(c));
        }
    }
}
