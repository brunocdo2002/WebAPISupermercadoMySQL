using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Models
{
    public class Usuários
    {
        public  int ID { get; set; }
        public string USUARIO { get; set; }
        public string SENHA { get; set; }
        public int NIVEL { get; set; }
    }
}
