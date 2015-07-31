
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using NeuronDocumentSync.Cypher;
using NeuronDocumentSync.Models;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Infrastructure.Extensions;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    [Authorize(Roles = IdentityConstants.SyncServiceRole)]
    [RoutePrefix("api/Import")]
    public class ImportController : ApiController
    {
        private WebDocumentConverter _docConverter;
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }

        private IRSACypher Cypher { get; set; }

        public ImportController(WebDocumentConverter docConverter, IDBUnitOfWork uow, IWebLogger logger,
            IRSACypher cypher)
        {
            _docConverter = docConverter;
            Database = uow;
            Log = logger;
            Cypher = cypher;
        }
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]CypheredDocument value)
        {
            ExportServiceDocument decryptedData;
            try
            {
                decryptedData = Cypher.DecryptAndDeserializeData<ExportServiceDocument>(value.CypheredData);
            }
            catch(Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);

                return response;
            }
            
            try
            {
                if ((decryptedData.ImagesInterpretation.Count == 0) && (decryptedData.PdfFileData == null))
                {
                    Log.Warn(string.Format(MainExceptionMessages.rs_ExportServiceDocumentHasNoData, decryptedData.NeuronDbDocumentId));
                    return Request.CreateResponse(HttpStatusCode.NoContent, WebApiMessages.DataIsEmpty);
                }


                var document = _docConverter.Convert(decryptedData);
                if (document != null)
                {
                    var inDbDoc =
                        Database.ServiceDocuments.GetQueryable().FirstOrDefault(p => (p.NeuronDbDocumentId == document.NeuronDbDocumentId) &&
                                                                                     (p.CreatDate >= document.CreatDate));
                    if (inDbDoc != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, WebApiMessages.DocIsExist);
                    }


                    document.DocumentOperations.Add(new DocumentLogOperation
                    {
                        OperationType = DocumentLogOperationType.Imported,
                        ConnectionIpAddress = Request.GetClientIpAddress() ?? string.Empty
                    });
                    Database.ServiceDocuments.Create(document);
                    Database.Save();
                    var response = Request.CreateResponse(HttpStatusCode.OK, WebApiMessages.Ok);

                    return response;
                }
                else
                {
                    Log.Error(string.Format(MainExceptionMessages.rs_DocumentConvertError, decryptedData.NeuronDbDocumentId));
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

        [Route("GetPublicKey")]
        public HttpResponseMessage GetPublicKey()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    Cypher.GetCurrentPublicKey(),
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }
    }
}