using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Models
{
    public class Balanco
    {
        public Int32 ID { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime DATA { get; set; }
        public string STATUS { get; set; }
        public Int32 ITENS { get; set; }
        public string CRIADAPOR { get; set; }
        public string OBS { get; set; }
    }
}
