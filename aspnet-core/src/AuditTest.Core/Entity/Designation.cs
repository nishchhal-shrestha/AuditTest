using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using AuditTest.CustomEntityHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.Entity
{
    [Audited]
    public class Designation : CustomEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
