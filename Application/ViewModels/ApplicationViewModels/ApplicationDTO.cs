using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ApplicationViewModels
{
    public class ApplicationDTO
    {
        public Guid UserID { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime AbsentDateRequested { get; set; }
        public Boolean? Approved { get; set;}
    }
}
