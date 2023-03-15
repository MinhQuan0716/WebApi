using Application.ViewModels.ApplicationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ApplicationDateTimeFilterDTO:ApplicationDTO
    {
        public ApplicationDateTimeFilterDTO() { }

        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; } = DateTime.MaxValue;
        public bool?  Approved { get; set; }
        public Guid UserID { get; set; }
    }
}
