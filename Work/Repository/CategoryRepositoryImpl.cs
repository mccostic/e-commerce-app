using Microsoft.EntityFrameworkCore;
using ecom.Data;
using ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Work.Repository;

namespace MoneStore.Work.Repository
{
    public class ContactMessageRepositoryImpl : IContactUsRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(ContactMessage contactMessage)
        {
            await _context.ContactMessages.AddAsync(contactMessage);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var contactMessage = _context.ContactMessages.Where(c => c.Id == id).FirstOrDefault();
            _context.ContactMessages.Remove(contactMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<ContactMessage> GetById(int id)
        {
            return await _context.ContactMessages.Where(c => c.Id == id).FirstOrDefaultAsync();
          
        }

        public async Task<IList<ContactMessage>> GetContactMessageList()
        {
            return await _context.ContactMessages.ToListAsync();
        }

        public async Task Update(ContactMessage contactMessage)
        {
            _context.Entry(contactMessage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
