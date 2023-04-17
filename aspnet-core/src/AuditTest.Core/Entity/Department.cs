using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.Entity
{
    public class Department : FullAuditedEntity<Guid>
    {
        [Audited]
        [StringLength(500)]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
