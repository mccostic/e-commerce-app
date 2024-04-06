using ecom.Models;
using ecom.Models.Dto;
using ecom.Work.Repository;
using MoneStore.Work.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Work.Managers
{
    public class ContactUsManager
    {
        private IContactUsRepository _contactUsRepository;

        public ContactUsManager(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        public async Task<IList<ContactMessage>> GetContactMessageList()
        {
            return await _contactUsRepository.GetContactMessageList();
      
        }

        public async Task CreateContactUsMessage(ContactMessage contactMessage)
        {
            await _contactUsRepository.Add(contactMessage);
        }

        public async Task DeleteContactUsMessage(int id)
        {
            await _contactUsRepository.Delete(id);
        }

        public async Task<ContactMessage> GetContactUsMessageById(int id)
        {
            return await _contactUsRepository.GetById(id);
        }

        public async Task EditCategory(ContactMessage category)
        {
            await _contactUsRepository.Update(category);
        }

       
    }
}
