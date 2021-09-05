using App.DbAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.DbAccess.Services
{
    public interface IProductReport
    {
        Task<IEnumerable<ProductView>> GetProductsAsync(string request);
    }
}
