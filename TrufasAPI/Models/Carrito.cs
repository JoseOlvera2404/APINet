using System.Text.Json.Serialization;
namespace TrufasAPI.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public List<DetalleCarrito> Detalles { get; set; }
    }
}