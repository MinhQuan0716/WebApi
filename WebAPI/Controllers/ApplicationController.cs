using Application.Interfaces;
using Application.ViewModels.ApplicationViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  
    public class ApplicationController : BaseController
    {
       private readonly IApplicationServices _services;
        public ApplicationController(IApplicationServices services)
        {
            _services = services;

        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication( [FromBody] ApplicationDTO applicationDTO)
        {
         await _services.CreateApplication(applicationDTO);
            return NoContent();
        }

        [HttpGet]
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var result = await _services.UpdateStatus(id, status);
            if (result)
            {
                return true;
            }
            return false;
        }
    }
}
