
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NeuronDocumentSync.Models;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    public class ImportController : ApiController
    {
        private WebDocumentConverter _docConverter;
        private IUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }

        public ImportController(WebDocumentConverter docConverter, IUnitOfWork uow, IWebLogger logger)
        {
            _docConverter = docConverter;
            Database = uow;
            Log = logger;
        }
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]ExportServiceDocument value)
        {
            try
            {
                if((value.ImagesInterpretation.Count == 0) && (value.PdfFileData == null))
                {
                    Log.Warn(string.Format(MainExceptionMessages.rs_ExportServiceDocumentHasNoData, value.NeuronDbDocumentId));
                    return Request.CreateResponse(HttpStatusCode.NoContent, WebApiMessages.DataIsEmpty);
                }


                var document = _docConverter.Convert(value);
                if (document != null)
                {
                    Database.ServiceDocuments.Create(document);
                    Database.Save();
                    var response = Request.CreateResponse(HttpStatusCode.OK, WebApiMessages.Ok);

                    return response;
                }
                else
                {
                    Log.Error(string.Format(MainExceptionMessages.rs_DocumentConvertError, value.NeuronDbDocumentId));
                    var response = Request.CreateResponse(HttpStatusCode.NotAcceptable);

                    return response;
                }


                
            }
            catch (Exception e)
            {
                Log.Error(MainExceptionMessages.rs_ExceptionInImportControllerPost, e);

                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);

                return response;
            }
        }

    }
}