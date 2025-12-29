using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasConcierto.Models
{
    public class Boleta
    {
        public int IdBoleta {  get; set; }
        public string NombreCliente {  get; set; }
        public string Lugar {  get; set; }
        public string Evento {  get; set; }
        public decimal Costo {  get; set; }
    }
}
