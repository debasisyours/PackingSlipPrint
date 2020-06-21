using Microsoft.Extensions.DependencyInjection;
using PackingSlip.Repository;
using PackingSlip.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackingSlip.Api
{
    public static class DependeciesExtension
    {
        public static void RegisterDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICustomerMembershipRepository, CustomerMembershipRepository>();
            serviceCollection.AddTransient<IPackingSlipRepository, PackingSlipRepository>();
            serviceCollection.AddTransient<IFreeProductRepository, FreeProductRepository>();

            serviceCollection.AddTransient<IPackingSlipValidator, PackingSlipValidator>();
            serviceCollection.AddTransient<IPackingSlipSaveService, PackingSlipSaveService>();
            serviceCollection.AddTransient<IFreeItemCheckerService, FreeItemCheckerService>();
        }
    }
}
