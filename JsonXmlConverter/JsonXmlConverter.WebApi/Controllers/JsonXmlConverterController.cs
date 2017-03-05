using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using JsonXmlConverter.Business;
using System.Text;

namespace JsonXmlConverter.WebApi.Controllers
{
    //WebApi controller to accept a file and send back the proper translation
    [EnableCors("*", "*", "POST")]
    public class JsonXmlConverterController : ApiController
    {
        private IJsonXmlConverter _converter;

        //default constructor
        public JsonXmlConverterController()
        {
            _converter = new DefaultJsonXmlConverter();
        }

        //dependecy injection constructor
        public JsonXmlConverterController(IJsonXmlConverter conv)
        {
            _converter = conv;
        }


        [HttpPost]
        public HttpResponseMessage GetJsonXmlTranslation()
        {
            //get content as string so we can inspect and convert it
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            StreamReader reader = new StreamReader(file.InputStream);
            string rawContent = reader.ReadToEnd();

            //easy call for conversion!
            IFormattedContent convertedContent = _converter.Convert(rawContent);

            //now build a new response and send back the converted content
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(getStream(convertedContent.Content));

            //label the response content type appropriately depending on the convertedContent
            switch (convertedContent.FileType)
            {
                case FileType.Json:
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    break;
                case FileType.Xml:
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                    break;
                default: //uh oh, reassign result to error
                    result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown file type.");
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    break;
            }

            return result;
        }

        private Stream getStream(string s)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(s);
            return new MemoryStream(byteArray);
        }
    }
}
