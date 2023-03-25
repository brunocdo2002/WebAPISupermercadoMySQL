using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Models
{
    public class ProdutosCotacao
    {
        public long CODBARRA { get; set; }
        public string DESCRICAO { get; set; }
        public double CUSTO { get; set; }
        public double VALOR { get; set; }
        public double ESTOQUE { get; set; }
        public string ULTFORNECEDOR { get; set; }
        public string ULTCOMPRA { get; set; }
        public string FORNECEDOR { get; set; }
        public double QTDCOMPRA { get; set; }
        public double VALORCOMPRA { get; set; }
        public string BONIFICACAO { get; set; }
        public string OBS { get; set; }

    }
}
