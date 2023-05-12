using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.FtpClient;

namespace WebAPISupermercadoMySql.Controllers
{
    public class ImageController : ApiController
    {

        private const string FTP_USERNAME = "bomprec2";
        private const string FTP_PASSWORD = "@Cgab1546";

        [HttpGet]
        public IHttpActionResult GetImage(string filename)
        {

            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                webClient.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                imageBytes = webClient.DownloadData("ftp://bompreco.online/web/IMAGENS/" + filename);
            }

            // Retornar a imagem como uma resposta HTTP
            var response = new HttpResponseMessage(HttpStatusCode.OK)

            {
                Content = new ByteArrayContent(imageBytes),
            };

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return ResponseMessage(response);
        }
    }
}

