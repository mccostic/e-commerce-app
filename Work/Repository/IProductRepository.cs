using ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Work.Repository
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(Product product);
        Task<Product> GetById(int id);
        Task<IList<Product>> GetProducts();
    }
}
