using Microsoft.EntityFrameworkCore;
using NeoRxTask.Controllers;
using NeoRxTask.Data;
using NeoRxTask.DTOs;
using NeoRxTask.Entities;
using NeoRxTask.Repositories.IRepositories;

namespace NeoRxTask.Repositories
{
    public class BusinessCardRepository : IBusinessCardRepository

    {
        private readonly AppDbContext _context;
        public BusinessCardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusinessCard>> GetAllAsync(int page, int pageSize)
        {
            return await _context.BusinessCard.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _context.BusinessCard.CountAsync();
        }
        public async Task<BusinessCard> GetByIdAsync(int id)
        {
            return await _context.BusinessCard.FindAsync(id);
        }

        public async Task AddAsync(BusinessCard card)
        {
            _context.BusinessCard.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id,BusinessCard card)
        {
            _context.BusinessCard.Update(card);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(BusinessCard card)
        {
            _context.BusinessCard.Remove(card);
            await _context.SaveChangesAsync();
        }

        //public async Task<List<BusinessCard>> FilterAsync(BusinessCard card)
        //{
        //    _context.BusinessCard.FilterAsync(card);
        //    await _context.SaveChangesAsync();

        //}

        //public async Task<List<BusinessCard>> FilterAsync(BusinessCard filter)
        //{
        //    IQueryable<BusinessCard> query = _context.BusinessCard.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(filter.Name))
        //        query = query.Where(x => x.Name.Contains(filter.Name));

        //    if (!string.IsNullOrWhiteSpace(filter.Email))
        //        query = query.Where(x => x.Email.Contains(filter.Email));

        //    if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
        //        query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));

        //    if (!string.IsNullOrWhiteSpace(filter.Gender))
        //        query = query.Where(x => x.Gender.Contains(filter.Gender));

        //    if (filter.DOB != default)
        //        query = query.Where(x => x.DOB.Date == filter.DOB.Date);

        //    return await query.ToListAsync();
        //}
        public IQueryable<BusinessCard> GetAll()
        {
            return _context.BusinessCard.AsQueryable();
        }

    }
}