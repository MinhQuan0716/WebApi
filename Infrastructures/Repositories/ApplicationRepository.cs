using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ApplicationReapository : GenericRepository<Applications>, IApplicationReapository
    {
        private readonly AppDbContext _appDbContext;
        public ApplicationReapository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
           _appDbContext= context;
    }
    }
}
