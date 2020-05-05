using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudCMSApi.IdentityModels
{
    public class IdentityAppContext:IdentityDbContext<AppUser,AppRole,int>
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options):base(options)
        {

        }
    }
}
