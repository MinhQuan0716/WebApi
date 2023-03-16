using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class DetailTrainingClassParticipateController : BaseController
    {
        private readonly IDetailTrainingClassParticipateService _detailTrainingClassParticipateService;

        public DetailTrainingClassParticipateController(IDetailTrainingClassParticipateService detailTrainingClassParticipateService)
        {
            _detailTrainingClassParticipateService = detailTrainingClassParticipateService;
        }

        [HttpPut]
        public async Task<IActionResult> IsTraining(Guid classId)
        {
            bool participation = await _detailTrainingClassParticipateService.IsTraining(classId);
            if (participation)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrainingClassParticipate(Guid userId, Guid classId)
        {
            var create = await _detailTrainingClassParticipateService.CreateTrainingClassParticipate(userId, classId);
            if (create != null)
            {
                return Ok();
            }
            return BadRequest();
        }
        /*public async Task<TraineeParticipationStatus> IsTraining(Guid classid)
        {
            var userid = _claimsService.GetCurrentUserId;
            //classid = GetCurrentClassId;
            DetailTrainingClassParticipate detail = await _unitOfWork.DetailTrainingClassParticipateRepository.GetDetailTrainingClassParticipateAsync(userid, classid);
            TraineeParticipationStatus status = detail.TraineeParticipationStatus;
            if (detail != null)
            {
                detail.TraineeParticipationStatus = TraineeParticipationStatus.Training;
            }
            return status;
        }*/
    }
}
