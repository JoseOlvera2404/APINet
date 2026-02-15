using System.Text.Json.Serialization;

namespace TrufasAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }
        public List<DetallePedido> Detalles { get; set; }
    }
}