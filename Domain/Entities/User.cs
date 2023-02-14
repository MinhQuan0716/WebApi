using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; } = null!;
        public DateTime LoginDate { get; set; }

        public ICollection<Syllabus> Syllabuses { get; set; }   
    }
}
