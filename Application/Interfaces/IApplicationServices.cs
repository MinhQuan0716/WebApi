using Application.ViewModels.ApplicationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationServices
    {
        public Task CreateApplication(ApplicationDTO applicationDTO);

        public Task<bool> UpdateStatus(Guid id, bool status);
    }
}
