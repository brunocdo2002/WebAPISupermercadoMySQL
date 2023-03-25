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
    public class CotacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CotacaoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{cotacao}")]
        public JsonResult Get(int cotacao)
        {
            string query = @"
                        SELECT *FROM `"+ cotacao +"`";

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

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        SELECT *FROM `ListasCompra`";

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

        [HttpPost("{cotacao}")]
        public JsonResult Post(Produtos produtos, int cotacao)
        {

            string query = @"
                INSERT IGNORE INTO `" + cotacao + "` (CODBARRA, DESCRICAO, CUSTO, VALOR, ESTOQUE, QTDCOMPRA," +
                "VALORCOMPRA, FORNECEDOR, BONIFICACAO, OBS) VALUES" +
                "(@CODBARRA, @DESCRICAO, @CUSTO, @VALOR, @ESTOQUE, @QTDCOMPRADA, @VALORCOMPRA, @FORNECEDOR," +
                "@BONIFICACAO, @OBS)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CODBARRA", produtos.CODBARRA);
                    myCommand.Parameters.AddWithValue("@DESCRICAO", produtos.DESCRICAO);
                    myCommand.Parameters.AddWithValue("@CUSTO", produtos.CUSTO);
                    myCommand.Parameters.AddWithValue("@VALOR", produtos.PRVENDA);
                    myCommand.Parameters.AddWithValue("@ESTOQUE", produtos.ESTOQ);
                    myCommand.Parameters.AddWithValue("@QTDCOMPRADA", "0");
                    myCommand.Parameters.AddWithValue("@VALORCOMPRA", "0.00");
                    myCommand.Parameters.AddWithValue("@FORNECEDOR", "SEM COMPRA");
                    myCommand.Parameters.AddWithValue("@BONIFICACAO", "-");
                    myCommand.Parameters.AddWithValue("@OBS", "-");

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Adicionado com Sucesso");
        }


        [HttpPut("{cotacao}")]
        public JsonResult Put(ProdutosCotacao produtos, int cotacao)
        {
            string query = @"
                        UPDATE `" + cotacao + "`SET" +
                        "QTDCOMPRA =@QTDCOMPRADA" +
                        "VALORCOMPRA = @VALORCOMPRA" +
                        "FORNECEDOR = @FORNCEDOR" +
                        "BONIFICAO = @BONIFICAO" +
                        "OBS = @OBS" +
                        "WHERE CODBARRA=@CODBARRA;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CODBARRA", produtos.CODBARRA);
                    myCommand.Parameters.AddWithValue("@QTDCOMPRADA", produtos.QTDCOMPRA);
                    myCommand.Parameters.AddWithValue("@VALORCOMPRA", produtos.VALORCOMPRA);
                    myCommand.Parameters.AddWithValue("@FORNECEDOR", produtos.FORNECEDOR);
                    myCommand.Parameters.AddWithValue("@BONIFICACAO", produtos.BONIFICACAO);
                    myCommand.Parameters.AddWithValue("@OBS", produtos.OBS);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }



        [HttpDelete("{cotacao}")]
        public JsonResult Delete(ProdutosCotacao produto, int cotacao)
        {

            string query = @"
                        DELETE FROM `" + cotacao + "`" +
                        "WHERE CODBARRA=@CODBARRA;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CODBARRA", produto.CODBARRA);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
