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
    public class DetailUnitLectureRepository : GenericRepository<DetailUnitLecture>,IDetailUnitLectureRepository
    {
        private readonly AppDbContext _AppDbContext;
        public DetailUnitLectureRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _AppDbContext = context;
        }
    }
}
