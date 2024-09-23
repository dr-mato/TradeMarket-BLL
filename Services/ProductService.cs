using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : CrudService<ProductModel>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            if (unitOfWork == null) { return; }
            _productRepository = unitOfWork.ProductRepository;
            _categoryRepository = unitOfWork.ProductCategoryRepository;
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            var neededProducts = new List<Product>();
            if (categoryModel == null) { return; }
            foreach(var productId in categoryModel.ProductIds)
            {
                var prod = await _productRepository.GetByIdAsync(productId);
                neededProducts.Add(prod);
            }
            var category = new ProductCategory
            {
                Id = categoryModel.Id,
                CategoryName = categoryModel.CategoryName,
                Products = neededProducts
            };
            await _categoryRepository.AddAsync(category);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new ProductCategoryModel(c));
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _productRepository.GetAllAsync();
            if (filterSearch == null) { return products.Select(p => new ProductModel(p)); }
            if (filterSearch.CategoryId != null)
            {
                products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId);
            }
            if (filterSearch.MinPrice != null)
            {
                products = products.Where(p => p.Price >= filterSearch.MinPrice);
            }   
            if (filterSearch.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= filterSearch.MaxPrice);
            }
            return products.Select(p => new ProductModel(p));
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _categoryRepository.DeleteByIdAsync(categoryId);
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            var neededProducts = new List<Product>();
            if (categoryModel == null) { return; }
            foreach (var productId in categoryModel.ProductIds)
            {
                var prod = await _productRepository.GetByIdAsync(productId);
                neededProducts.Add(prod);
            }
            var category = new ProductCategory
            {
                Id = categoryModel.Id,
                CategoryName = categoryModel.CategoryName,
                Products = neededProducts
            };
            _categoryRepository.Update(category);
        }
    }
}
