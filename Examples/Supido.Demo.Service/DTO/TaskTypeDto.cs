using Supido.Business.Attributes;

namespace Supido.Demo.Service.DTO
{
    [Dto]
    public class TaskTypeDto
    {
        public int TaskTypeId { get; set; }

        public string Name { get; set; }
    }
}