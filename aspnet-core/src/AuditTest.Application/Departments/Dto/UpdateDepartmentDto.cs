using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AuditTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.Departments.Dto
{
    [AutoMapTo(typeof(Department))]
    public class UpdateDepartmentDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
