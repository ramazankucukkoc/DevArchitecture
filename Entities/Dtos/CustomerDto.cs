using System;

namespace Entities.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastUpdatedUserId { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool Status { get; set; }
        public bool isDeleted { get; set; }
        public string Code { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

    }
}
