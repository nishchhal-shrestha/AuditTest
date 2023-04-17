using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.CustomEntityHelper
{
    public class CustomEntity : FullAuditedEntity<string>
    {
    }
}
