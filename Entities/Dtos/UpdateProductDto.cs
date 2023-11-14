using Entities.Concrete;

namespace Entities.Dtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ColorId { get; set; }
        public Size Size { get; set; }
    }
}
