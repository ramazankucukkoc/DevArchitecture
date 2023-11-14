namespace Entities.Concrete
{
    public class Product: BaseEntityProject
    {
        public string Name { get; set; }
        public int ColorId { get; set; }
        public Size Size { get; set; }

    }
    public enum Size
    {
        S=1,
        M=2,
        L=3,
        XL=4
    }
}
