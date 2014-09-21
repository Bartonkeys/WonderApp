using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WonderApp.Core.CloudImage;

namespace WonderApp.Controllers
{
    [RoutePrefix("image")]
    public class ImageController : BaseApiController
    {
        protected CloudImageService CloudImageService;
        static readonly string ServerUploadFolder = Path.GetTempPath();

        [Inject]
        public ImageController(ICloudImageProvider cloudImageProvider)
        {
            CloudImageService = new CloudImageService(cloudImageProvider);
        }

        // GET: Image
        [Route("upload")]
        public async Task<HttpResponseMessage> PostImageGuessMultiPart()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);

            // Read the MIME multipart asynchronously content using the stream provider we just created.
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            var deals = Mapper.Map<List<DealModel>>(DataContext.Deals);
            return Request.CreateResponse(HttpStatusCode.OK, deals);
        }
    }
}