using NeoRxTask.DTOs;
using NeoRxTask.Entities;
using NeoRxTask.Repositories;
namespace NeoRxTask.Repositories.IRepositories
{
    public interface IBusinessCardRepository
    {
        Task<List<BusinessCard>> GetAllAsync(int page,int pagesize);
        Task<int> CountAsync();
        Task<BusinessCard> GetByIdAsync(int id);
        Task AddAsync(BusinessCard card);
        Task UpdateAsync(int id,BusinessCard card);
        Task DeleteAsync(BusinessCard card);
        //Task<List<BusinessCard>> FilterAsync(BusinessCard card);
        IQueryable<BusinessCard> GetAll();
    }
}
