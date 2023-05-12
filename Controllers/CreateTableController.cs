using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTableController : Controller
    {
        private readonly IConfiguration _configuration;
        public CreateTableController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT Usuarios.USUARIO, information_schema.tables.AUTO_INCREMENT FROM Usuarios, information_schema.tables WHERE TABLE_NAME = 'Usuarios' AND ID = '"+id+"'";

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
            if (table.Rows.Count == 0)
            {
                return new JsonResult("Nenhum usuário encontrado");
            }
            else
            {
                return new JsonResult(table);
            }
        }
    }
}
