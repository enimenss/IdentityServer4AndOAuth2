using IdentityServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Entities;
using IdentityServer.Helpers;

namespace IdentityServer.Repositories
{
    public class ProviderRepository : IProviderRepository
    {

        public  IEnumerable<Provider> Get()
        {
            return ProviderDataSource.GetProviders();
        }
    }
}
