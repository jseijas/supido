using Supido.Business.Attributes;

namespace Supido.Demo.Service.DTO
{
    [Dto]
    public class ClientDto
    {
        public int ClientId { get; set; }

        public string Name { get; set; }
    }
}