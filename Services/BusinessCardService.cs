using Microsoft.EntityFrameworkCore;
using NeoRxTask.Controllers;
using NeoRxTask.DTOs;
using NeoRxTask.Entities;
using NeoRxTask.Repositories.IRepositories;
using NeoRxTask.Services.IServices;


namespace NeoRxTask.Services
{
    public class BusinessCardService : IBusinessCardService
    {

        private readonly IBusinessCardRepository _repo;
        private readonly IWebHostEnvironment _env;
        public BusinessCardService(IBusinessCardRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<(List<BusinessCard>, int)> GetAllAsync(int page, int pageSize)
        {
            var data = await _repo.GetAllAsync(page, pageSize);
            var count = await _repo.CountAsync();
            return (data, count);
        }
        public async Task<BusinessCard> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<BusinessCard> CreateAsync(CreateBusinessCardDto dto)
        {
            var card = new BusinessCard
            {
                Name = dto.Name ?? string.Empty,
                Gender = dto.Gender ?? string.Empty,
                DOB = dto.DOB,
                Email = dto.Email ?? string.Empty,
                PhoneNumber = dto.PhoneNumber ?? string.Empty,
                Address = dto.Address ?? string.Empty
            };

            await _repo.AddAsync(card);
            return card;
        }
        public async Task<BusinessCard> UpdateAsync(int id, UpdateBusinessCardDto dto)
        {
            var existingCard = await _repo.GetByIdAsync(id);

            if (existingCard == null)
                throw new Exception("Business card not found");
            existingCard.Name = dto.Name ?? string.Empty;
            existingCard.Gender = dto.Gender ?? string.Empty;
            existingCard.DOB = dto.DOB;
            existingCard.Email = dto.Email ?? string.Empty;
            existingCard.PhoneNumber = dto.PhoneNumber ?? string.Empty;
            existingCard.Address = dto.Address ?? string.Empty;

            existingCard.Id = id;
            await _repo.UpdateAsync(id, existingCard);
            return existingCard;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var card = await _repo.GetByIdAsync(id);
            if (card == null)
                return false;

            await _repo.DeleteAsync(card);
            return true;
        }


        public async Task<List<BusinessCard>> FilterAsync(BusinessCardFilterDto filter)
        {
            var query = _repo.GetAll();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(x => x.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));

            if (!string.IsNullOrWhiteSpace(filter.Gender))
                query = query.Where(x => x.Gender.Contains(filter.Gender));

            if (filter.DOB.HasValue)
                query = query.Where(x => x.DOB.Date == filter.DOB.Value.Date);

            return await query.ToListAsync();
        }
        //    public async Task<List<BusinessCard>> GetAllAsync() { }
        //}


        public async Task<BusinessCard> CreateWithPhotoAsync(CreateBusinessCardDto dto)
        {
            string? photoPath = null;

            if (dto.Photo != null)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(dto.Photo.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                    throw new Exception("Only JPG, PNG, GIF files are allowed");

                if (dto.Photo.Length > 1_000_000)
                    throw new Exception("Photo must be less than 1MB");

                // Use _env.WebRootPath instead of Directory.GetCurrentDirectory()
                var fileName = Guid.NewGuid().ToString() + ext;
                var folderPath = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Photo.CopyToAsync(stream);
                }

                photoPath = "/images/" + fileName;
            }

            var card = new BusinessCard
            {
                Name = dto.Name ?? string.Empty,
                Gender = dto.Gender ?? string.Empty,
                DOB = dto.DOB,
                Email = dto.Email ?? string.Empty,
                PhoneNumber = dto.PhoneNumber ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                PhotoPath = photoPath
            };

            await _repo.AddAsync(card);
            return card;
        }
    }
}
