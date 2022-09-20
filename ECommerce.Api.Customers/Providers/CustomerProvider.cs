using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer = ECommerce.Api.Customers.Models.Customer;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomersProvider
    {
        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomerDbContext dbContext, ILogger<CustomerProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer { Id = 1, Name = "Max Smith", Address = "Washinton 27 Street" });
                dbContext.Customers.Add(new Db.Customer { Id = 2, Name = "Lenis Pride", Address = "New York 22 Street" });
                dbContext.Customers.Add(new Db.Customer { Id = 3, Name = "Marc Washington ", Address = "18th Street NW" });
                dbContext.Customers.Add(new Db.Customer { Id = 4, Name = "Helena Jones", Address = "Embassy Row 17" });

                dbContext.SaveChanges();
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation("Querying customers");
                var customers = await dbContext.Customers.ToListAsync();

                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Customer>>(customers);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool isSuccess, Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying customer by Id");
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Customer>(customer);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
