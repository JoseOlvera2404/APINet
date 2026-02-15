using System.Text.Json.Serialization;
namespace TrufasAPI.Models
{
    public class DetalleCarrito
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        [JsonIgnore]
        public Carrito Carrito { get; set; }
        public Producto Producto { get; set; }
    }
}