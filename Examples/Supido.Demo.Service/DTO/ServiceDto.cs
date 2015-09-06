using Supido.Business.Attributes;

namespace Supido.Demo.Service.DTO
{
    [Dto]
    public class ServiceDto
    {
        public int ServiceId { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }
    }
}