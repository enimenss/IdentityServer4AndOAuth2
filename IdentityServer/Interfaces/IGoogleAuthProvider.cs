using IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Interfaces
{
    public interface IGoogleAuthProvider:IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
