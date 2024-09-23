using Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Entities;

namespace Business.Services
{
    public class CrudService<TModel> : ICrud<TModel> where TModel : BaseEntity
    {
        private readonly IRepository<TModel> _repository;
        public CrudService(IRepository<TModel> repository) 
        {
            _repository = repository;
        }
        public CrudService()
        {
        }
        public async Task AddAsync(TModel model)
        {
            await _repository.AddAsync(model);
        }
        public async Task DeleteAsync(int modelId)
        {
            await _repository.DeleteByIdAsync(modelId);
        }

        public Task<IEnumerable<TModel>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<TModel> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task UpdateAsync(TModel model)
        {
            _repository.Update(model);
            return Task.CompletedTask;
        }
    }
}
