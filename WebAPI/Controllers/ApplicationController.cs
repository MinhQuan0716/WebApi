using Application.Interfaces;
using Application.ViewModels.ApplicationViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  
    public class ApplicationController : BaseController
    {
       private readonly IApplicationService _service;
        public ApplicationController(IApplicationService services)
        {
            _service = services;

        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication( [FromBody] ApplicationDTO applicationDTO)
        {
         await _service.CreateApplication(applicationDTO);
            return NoContent();
        }

        [HttpGet]
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var result = await _service.UpdateStatus(id, status);
            if (result)
            {
                return true;
            }
            return false;
        }
    }
}
