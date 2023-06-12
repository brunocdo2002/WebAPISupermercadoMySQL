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

        //retorna a lista de produtos de determinada cotação
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

        //retorna a lista de cotações
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        SELECT *FROM `ListasCompras`";

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

        //inseri produto faltante na lista da cotação
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

        //cria uma nova lista com triggers para fazer contagem do total de itens
        [HttpPost]
        public JsonResult Post(Listas listas)
        {

            string query = @"
                        START TRANSACTION;" +
                       "INSERT INTO ListasCompra (DESCRICAO, DATA, STATUS, ITENS, CRIADOR, OBS) VALUES (@DESCRICAO, @DATA, 'ATIVO', '0', @CRIADOR, @OBS);" +
                       "CREATE TABLE `"+listas.ID+"` (`CODBARRA` BIGINT(14) NOT NULL , `DESCRICAO` VARCHAR(100) NOT NULL , " +
                       "`CUSTO` DECIMAL(6,4) NOT NULL , `VALOR` DECIMAL(6,2) NOT NULL , `ESTOQUE` DECIMAL(6,4) NOT NULL , " +
                       "`FORNECEDOR` VARCHAR(50) NOT NULL , `QTDCOMPRA` INT NOT NULL , `VALORCOMPRA` DECIMAL(6,2) NOT NULL , " +
                       "`BONIFICACAO` VARCHAR(50) NOT NULL , `OBS` VARCHAR(100) NOT NULL ) ENGINE = MyISAM;" +
                       "COMMIT;" +
                       "DELIMITER $$" +
                       "CREATE TRIGGER depoisDELETE_" + listas.ID + "" +
                       "BEFORE DELETE ON `" + listas.ID + "`" +
                       "FOR EACH ROW BEGIN" +
                       "UPDATE `Listas`"+
                       "SET ITENS = ITENS - 1 WHERE ID = '" + listas.ID + "'; END$$" +
                       "DELIMITER $"+
                       "CREATE TRIGGER depoisINSERT_" + listas.ID + "" +
                       "BEFORE INSERT ON `" + listas.ID + "`" +
                       "FOR EACH ROW BEGIN"+
                       "UPDATE `Listas`"+
                       "SET ITENS = ITENS + 1 WHERE ID = '" + listas.ID + "'; END$;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ID", listas.ID);
                    myCommand.Parameters.AddWithValue("@DESCRICAO", listas.DESCRICAO);
                    myCommand.Parameters.AddWithValue("@DATA", listas.DATA);
                    myCommand.Parameters.AddWithValue("@CRIADOR", listas.CRIADOR);
                    myCommand.Parameters.AddWithValue("@OBS", listas.OBS);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Adicionado com Sucesso");
        }

        //atualiza os dados com base na compra efetuada
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
            string sqlDataSource = _configuration.GetConnectionString("ProdutosConn");
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


        //deleta o item selecionado da lista
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
