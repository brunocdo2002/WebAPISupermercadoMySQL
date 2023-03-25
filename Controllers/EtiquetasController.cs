using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAPISupermercadoMySql.Models;

namespace WebAPISupermercadoMySql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiquetasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EtiquetasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        SELECT *FROM
                        Etiqueta
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost()]
        public JsonResult Post(Etiqueta etiqueta)
        {

            string query = @"
                INSERT INTO `Etiqueta` (CODBARRA, COD, COLETOR) VALUES" +
                "(@CODBARRA, @COD, @COLETOR)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CODBARRA", etiqueta.CODBARRA);
                    myCommand.Parameters.AddWithValue("@COD", etiqueta.COD);
                    myCommand.Parameters.AddWithValue("@COLETOR", etiqueta.COLETOR);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Adicionado com Sucesso");
        }
    }
}
