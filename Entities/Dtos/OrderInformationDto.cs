using System;

namespace Entities.Dtos
{
    public class OrderInformationDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public int Count { get; set; }//Şipariş adeti
        public bool isDeleted { get; set; }
        public string LastUpdatedDate { get; set; }
    }
}
