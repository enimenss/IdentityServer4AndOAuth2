using IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
        

    }
}
