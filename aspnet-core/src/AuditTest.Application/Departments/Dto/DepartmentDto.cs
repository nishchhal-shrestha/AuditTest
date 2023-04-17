using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AuditTest.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.Departments.Dto
{
    [AutoMapFrom(typeof(AuditTest.Entity.Department))]
    public class DepartmentDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
