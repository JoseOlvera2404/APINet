namespace TrufasAPI.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Stock { get; set; }

        public Producto Producto { get; set; }
    }
}