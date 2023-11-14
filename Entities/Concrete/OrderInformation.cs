namespace Entities.Concrete
{
    public class OrderInformation:BaseEntityProject
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Count { get; set; }//Şipariş adeti

    }
}
