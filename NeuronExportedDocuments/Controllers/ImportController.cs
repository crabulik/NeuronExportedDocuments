
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NeuronDocumentSync.Models;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    public class ImportController : ApiController
    {
        private WebDocumentConverter _docConverter;
        private IUnitOfWork Database { get; set; }

        public ImportController(WebDocumentConverter docConverter, IUnitOfWork uow)
        {
            _docConverter = docConverter;
            Database = uow;
        }
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ExportServiceDocument value)
        {
            if ((value.DeliveryPhone == string.Empty))
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, WebApiMessages.NoPhone);
            }

            if((value.ImagesInterpretation.Count == 0) && (value.PdfFileData == null))
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, WebApiMessages.DataIsEmpty);
            }


            var document = _docConverter.Convert(value);
            Database.ServiceDocuments.Create(document);
            Database.Save();


            var response = Request.CreateResponse(HttpStatusCode.OK, WebApiMessages.Ok);

            return response;
        }

    }
}