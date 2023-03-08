using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.AuditModels.AuditSubmissionModels.CreateModels;
using Application.ViewModels.AuditModels.AuditSubmissionModels.UpdateModels;
using Application.ViewModels.GradingModels;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

namespace WebAPI.Controllers
{
    public class AuditSubmissionController : BaseController
    {
        private readonly IAuditSubmissionService auditSubmissionService;
        private readonly IGradingService gradingService;
        private readonly IAuditPlanService auditPlanService;
   
        public AuditSubmissionController(IAuditSubmissionService auditSubmissionService, IGradingService gradingService, IAuditPlanService auditPlanService)
        {
            this.auditSubmissionService = auditSubmissionService;
            this.gradingService = gradingService;
            this.auditPlanService = auditPlanService;
        }

        [HttpPost]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.ClassPermission), nameof(PermissionEnum.Create))]
        public async Task<IActionResult> Create(CreateAuditSubmissionDTO createAuditSubmissionDTO, Guid detailTrainingClassParticipate)
        {
            var result = await auditSubmissionService.CreateAuditSubmission(createAuditSubmissionDTO);
            if (result is not null)
            {
                if(auditPlanService is not null && gradingService is not null)
                {
                   
                    var auditPlan = await auditPlanService.GetAuditPlanById(result.AuditPlanId);
                    if (auditPlan is not null)
                    {
                        string letterGrade = "";
                        if (result.TotalGrade < 10 && result.TotalGrade >= 8) letterGrade = "A";
                        else if (result.TotalGrade < 8 && result.TotalGrade >= 6) letterGrade = "B";
                        else if (result.TotalGrade < 6 && result.TotalGrade >= 4) letterGrade = "C";
                        else if (result.TotalGrade < 4 && result.TotalGrade <= 2) letterGrade = "D";
                        else letterGrade = "F";

                        var gradingModel = new GradingModel
                        {
                            NumericGrade = (int?)result.TotalGrade,
                            LectureId = auditPlan.LectureId
                                                             ,
                            DetailTrainingClassParticipateId = detailTrainingClassParticipate,
                            LetterGrade = letterGrade
                        };
                        await gradingService.CreateGradingAsync(gradingModel);
                        return Ok(result);

                    }
                    else return BadRequest("Can not found AuditPlan");
                } else return BadRequest("Not have auditPlan service and GradingService");
                
            }
            else return BadRequest("Can not create Submission");
           

            
            
        }
          
    

    [HttpGet]
        public async Task<IActionResult> GetDetail(Guid auditSubmissionId)
        {
            var result = await auditSubmissionService.GetAuditSubmissionDetail(auditSubmissionId);
            if (result is not null) return Ok(result);
            else return BadRequest();

        }

        [HttpDelete]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.ClassPermission), nameof(PermissionEnum.Modifed))]
        public async Task<IActionResult> Delete(Guid auditSubmissionId)
        {
            var result = await auditSubmissionService.DeleteSubmissionDetail(auditSubmissionId);
            if (result) return NoContent();
            else return BadRequest();
        }

        [HttpPut]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.ClassPermission), nameof(PermissionEnum.Modifed))]
        public async Task<IActionResult> Update(UpdateSubmissionDTO updateSubmissionDTO)
        {
            var result = await auditSubmissionService.UpdateSubmissionDetail(updateSubmissionDTO);
            if (result) return NoContent();
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByAuditPlan(Guid auditPLanId)
        {
            var result = await auditSubmissionService.GetAllAuditSubmissionByAuditPlan(auditPLanId);
            if (result is not null) return Ok(result);
            else return BadRequest();
        }
    }
}
