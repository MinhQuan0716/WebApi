using Application.Interfaces;
using Application.ViewModels.AuditModels;
using Application.ViewModels.AuditModels.UpdateModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class AuditPlanController : BaseController
    {
        private readonly IAuditPlanService auditPlanService;
        public AuditPlanController(IAuditPlanService auditPlanService)
        {
            this.auditPlanService = auditPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuditDTO createDTO)
        {
            var result = await auditPlanService.CreateAuditPlan(createDTO);
            if (result is not null) return Ok(result);
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid auditPlanId)
        {
            var result = await auditPlanService.ViewDetailAuditPlan(auditPlanId);
            if (result != null) return Ok(result);
            else return BadRequest();
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid auditPlanId)
        {
            var result = await auditPlanService.DeleteAuditPlan(auditPlanId);
            if (result) return NoContent();
            else return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuditDTO updateAuditDTO)
        {
            var result = await auditPlanService.UpdateAuditPlan(updateAuditDTO);
            if (result) return NoContent();
            else return BadRequest();
        }
    }
}
