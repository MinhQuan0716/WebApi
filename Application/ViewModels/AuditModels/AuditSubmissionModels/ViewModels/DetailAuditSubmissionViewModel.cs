﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AuditModels.AuditSubmissionModels.ViewModels
{
    public class DetailAuditSubmissionViewModel
    {
        public string Question { get; set; } = default!;
        public string Answer { get; set; } = default!;
    }
}
