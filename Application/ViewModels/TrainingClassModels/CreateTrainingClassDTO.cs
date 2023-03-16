using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingClassModels
{
    public partial class CreateTrainingClassDTO
    {
        public string Name { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid LocationID { get; set; }
        public Guid TrainingProgramId { get; set; }
        public string StatusClassDetail { get; set; }
        public string Code { get; set; }
        public string? Attendee { get; set; }
        public string Branch { get; set; }
        // public Guid UserId { get; set; }
    }
    /// <summary>
    /// bổ sung mới
    /// </summary>
    public partial class CreateTrainingClassDTO
    {
        public ICollection<AdminsDTO> Admins { get; set; }
        public string fsu { get; set; }
        //AttendeesDTO
        public AttendeesDTO Attendees { get; set; }
        //Time frame
        public TimeFrameDTO TimeFrame { get; set; }
    }
    public class TimeFrameDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ICollection<DateOnly>? HighlightedDates { get; set; }
    }
    public class AttendeesDTO
    {
        public int AttendeesPlannedNumber { get; set; }
        public int AttendeesAcceptedNumber { get; set; }
        public int AttendeesActualNumber { get; set; }

    }
    public class AdminsDTO
    {
        public Guid AdminID { get; set; }
    }
}
