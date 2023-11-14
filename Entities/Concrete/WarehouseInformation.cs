namespace Entities.Concrete
{
    public class WarehouseInformation:BaseEntityProject
    {
        public int ProductId { get; set; }
        public int Count { get; set; } //adet
        public bool ReadyForSale { get; set; }//SatışaHazırmı?
    }
}
