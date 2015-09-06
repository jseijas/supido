using Supido.Business.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supido.Demo.Service.DTO
{
    [Dto]
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }
    }
}