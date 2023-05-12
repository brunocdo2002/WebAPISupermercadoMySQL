using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPISupermercadoMySql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagensController : Controller
    {
        private const string FTP_HOST = "ftp.bompreco.online";
        private const string FTP_USERNAME = "bomprec2";
        private const string FTP_PASSWORD = "@Cgab1546";

        private readonly IConfiguration _configuration;
        public ImagensController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{filename}")]
        public JsonResult Get(string filename)
        {
            FtpClient ftp = new FtpClient();
            ftp.Host = FTP_HOST;
            ftp.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
            ftp.Connect();
            byte[] imageBytes;

            Stream ftpStream = ftp.OpenRead("/bompreco.online/web/IMAGENS/" + filename);

            imageBytes = ConverteStreamToByteArray(ftpStream);

            // Retornar a imagem como uma resposta HTTP
            var response = new HttpResponseMessage(HttpStatusCode.OK)

            {
                Content = new ByteArrayContent(imageBytes),
            };

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            
            return new JsonResult(imageBytes);
        }

        public static byte[] ConverteStreamToByteArray(Stream stream)
        {
            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream())
            {
                int bit;
                while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write(byteArray, 0, bit);
                }
                return mStream.ToArray();
            }
        }
    }
}
