namespace Entities.Dtos
{
    public class UpdateWarehouseInformationDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; } //adet
        public bool ReadyForSale { get; set; }//SatışaHazırmı?
    }
}
