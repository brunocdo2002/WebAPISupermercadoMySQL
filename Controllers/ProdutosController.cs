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
    public class ProdutosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProdutosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        SELECT
                        *FROM ProdutosTeste
            ";
            //`CODBAR`, `DESCRICAO`, `CUSTO`, `PRVENDA`, `PRVENDA02`, `EMITE`, `CODPROD`, `BALANCA`, `ESTOQ`, `PULTQTD`, `ULTQTD`, `PULTFORNECEDOR`, `PULTCOMPRA`, `ULTFORNECEDOR`, `ULTCOMPRA`, `QTPRVEND1`, `QTDEEMBAL`, `COLETADO`
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.CommandTimeout = 1000;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }


        //[HttpPost("{cotacao}")]
        //public JsonResult Post(Produtos produtos, int cotacao)
        //{
            
        //    string query = @"
        //        INSERT IGNORE INTO `"+ cotacao +"` (CODBARRA, DESCRICAO, CUSTO, VALOR, ESTOQUE, QTDCOMPRA,"+ 
        //        "VALORCOMPRA, FORNECEDOR, BONIFICACAO, OBS) VALUES"+
        //        "(@CODBARRA, @DESCRICAO, @CUSTO, @VALOR, @ESTOQUE, @QTDCOMPRADA, @VALORCOMPRA, @FORNECEDOR,"+
        //        "@BONIFICACAO, @OBS)";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
        //    MySqlDataReader myReader;
        //    using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        //    {
        //        mycon.Open();
        //        using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
        //        {
        //            myCommand.Parameters.AddWithValue("@CODBARRA", produtos.CODBARRA);
        //            myCommand.Parameters.AddWithValue("@DESCRICAO", produtos.DESCRICAO);
        //            myCommand.Parameters.AddWithValue("@CUSTO", produtos.CUSTO);
        //            myCommand.Parameters.AddWithValue("@VALOR", produtos.PRVENDA);
        //            myCommand.Parameters.AddWithValue("@ESTOQUE", produtos.ESTOQ);
        //            myCommand.Parameters.AddWithValue("@QTDCOMPRADA", "0");
        //            myCommand.Parameters.AddWithValue("@VALORCOMPRA", "0.00");
        //            myCommand.Parameters.AddWithValue("@FORNECEDOR", "SEM COMPRA");
        //            myCommand.Parameters.AddWithValue("@BONIFICACAO", "-");
        //            myCommand.Parameters.AddWithValue("@OBS", "-");

        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);

        //            myReader.Close();
        //            mycon.Close();
        //        }
        //    }

        //    return new JsonResult("Adicionado com Sucesso");
        //}


        [HttpPut]
        public JsonResult Put(Produtos produtos)
        {
            string query = @"
                        UPDATE ProdutosTeste SET"+
                        "PULTQTD = @PULTQTD" +
                        "ULTQTD = @ULTQTD" +
                        "PULTFORNECEDOR = @PULTFORNECEDOR" +
                        "PULTCOMPRA = @PULTCOMPRA"+
                        "ULTFORNECEDOR = @ULTFORNECEDOR" +
                        "ULTCOMPRA = PULTCOMPRA" +
                        "WHERE CODBARRA=@CODBARRA;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@PULTQTD", produtos.PULTQTD);
                    myCommand.Parameters.AddWithValue("@ULTQTD", produtos.ULTQTD);
                    myCommand.Parameters.AddWithValue("@PULTFORNECEDOR", produtos.PULTFORNECEDOR);
                    myCommand.Parameters.AddWithValue("@PULTCOMPRA", produtos.PULTCOMPRA);
                    myCommand.Parameters.AddWithValue("@ULTFORNECEDOR", produtos.ULTFORNECEDOR);
                    myCommand.Parameters.AddWithValue("@ULTCOMPRA", produtos.ULTCOMPRA);
                    myCommand.Parameters.AddWithValue("@CODBARRA", produtos.CODBARRA);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }



        //[HttpDelete("{cotacao}/{CODBARRA}")]
        //public JsonResult Delete(int cotacao, long CODBARRA)
        //{
            
        //    string query = @"
        //                DELETE FROM `"+ cotacao +"`"+
        //                "WHERE CODBARRA=@CODBARRA;";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
        //    MySqlDataReader myReader;
        //    using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
        //    {
        //        mycon.Open();
        //        using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
        //        {
        //            myCommand.Parameters.AddWithValue("@CODBARRA", CODBARRA);

        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);

        //            myReader.Close();
        //            mycon.Close();
        //        }
        //    }

        //    return new JsonResult("Deleted Successfully");
        //}

    }
}
