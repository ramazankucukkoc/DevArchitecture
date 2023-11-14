using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class Customer : BaseEntityProject, IEntity
    {
        public Customer()
        {
            if (Id == 0)
                CreatedDate = DateTime.Now;

            LastUpdatedDate = DateTime.Now;
            Status = true;
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
