using ECommerce.Api.Products.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool isSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
    }
}
