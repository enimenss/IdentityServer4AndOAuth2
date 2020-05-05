using IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Interfaces
{
    public interface IFacebookAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
