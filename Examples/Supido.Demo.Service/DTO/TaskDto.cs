using Supido.Business.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supido.Demo.Service.DTO
{
    [Dto]
    public class TaskDto
    {
        public int TaskId { get; set; }

        public int ServiceId { get; set; }

        public int TaskTypeId { get; set; }

        public int AssignedUserId { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }
    }
}