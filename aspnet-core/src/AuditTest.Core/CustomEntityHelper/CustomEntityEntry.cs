using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.CustomEntityHelper
{
    public class CustomEntityEntry<T> where T : CustomEntity
    {
        public T oldValue { get; protected set; }
        public T newValue { get; protected set; }
        public EntityChangeType entityChangeType { get; protected set; }

        public CustomEntityEntry(T oldValue, T newValue, EntityChangeType entityChangeType)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.entityChangeType = entityChangeType;
        }
    }
}
