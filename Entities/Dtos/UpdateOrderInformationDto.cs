namespace Entities.Dtos
{
    public class UpdateOrderInformationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Count { get; set; }//Şipariş adeti
    }
}
