using Core.Entities;

namespace Entities.Dtos
{
    public class UpdateCustomerDto:IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
