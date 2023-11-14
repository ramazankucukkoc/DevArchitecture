namespace Entities.Dtos
{
    public class WarehouseInformationDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; } //adet
        public bool ReadyForSale { get; set; }//SatışaHazırmı?
        public bool isDeleted { get; set; }

    }
}
