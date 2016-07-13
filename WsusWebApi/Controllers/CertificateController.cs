using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Web.Http;

namespace WsusGroupWS.Controllers
{
    public class CertificateController : ApiController
    {
        
        public readonly string certificate = WebConfigurationManager.AppSettings["CertPath"].ToString();

        
        public HttpResponseMessage GetCertificate()
        {
            if (!File.Exists(certificate))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(certificate, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = new FileInfo(certificate).Name;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }

    }
}
