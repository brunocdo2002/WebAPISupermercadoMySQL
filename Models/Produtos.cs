using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Models
{
    public class Produtos
    {
        public long CODBARRA { get; set; }
        public string DESCRICAO { get; set; }
        public double CUSTO { get; set; }
        public double PRVENDA { get; set; }
        public double PRVENDA2 { get; set; }
        public int EMITE { get; set; }
        public int CODPROD { get; set; }
        public string BALANCA { get; set; }
        public int ESTOQ { get; set; }
        public int PULTQTD { get; set; }
        public int ULTQTD { get; set; }
        public string PULTFORNECEDOR { get; set; }
        public DateTime PULTCOMPRA { get; set; }
        public string ULTFORNECEDOR { get; set; }
        public DateTime ULTCOMPRA { get; set; }
        public int QTDPRVEND1 { get; set; }
        public int QTDEEMBAL { get; set; }
    }
}
