using NeoRxTask.Entities;
using NeoRxTask.DTOs;

namespace NeoRxTask.Services.IServices
{
    public interface IBusinessCardService
    {
        Task<(List<BusinessCard>, int totalCount)> GetAllAsync(int page, int pageSize);
        Task<BusinessCard> GetByIdAsync(int id);
        Task<BusinessCard> CreateAsync(CreateBusinessCardDto dto);
        Task<BusinessCard> UpdateAsync(int id,UpdateBusinessCardDto dto);
        Task<bool> DeleteAsync(int id);
        
        Task<List<BusinessCard>> FilterAsync(BusinessCardFilterDto filter);
        //Task<List<BusinessCard>> GetAllAsync();
        Task<BusinessCard> CreateWithPhotoAsync(CreateBusinessCardDto dto);
    }

}
