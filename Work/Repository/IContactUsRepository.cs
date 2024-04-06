using ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Work.Repository
{
    public interface IContactUsRepository
    {
        Task<ContactMessage> GetById(int id);
        Task Add(ContactMessage category);
        Task Delete(int categoryId);
        Task<IList<ContactMessage>> GetContactMessageList();
        Task Update(ContactMessage category);
    }
}
