using Moov2.Orchard.Azure.PassThrough.Services.FileSystems;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using System.Web.Mvc;

namespace Moov2.Orchard.Azure.PassThrough.Controllers
{
    [OrchardFeature(Constants.PassThroughMediaFeatureName)]
    public class MediaController : Controller
    {
        private readonly IMediaPathProcessor _mediaPathProcessor;
        private readonly IStorageProvider _storageProvider;

        public MediaController(IMediaPathProcessor mediaPathProcessor, IStorageProvider storageProvider)
        {
            _mediaPathProcessor = mediaPathProcessor;
            _storageProvider = storageProvider;
        }

        public ActionResult Index(string mediaPath)
        {
            mediaPath = _mediaPathProcessor.CleanPath(mediaPath);
            if (!_storageProvider.FileExists(mediaPath))
            {
                return HttpNotFound();
            }
            var file = _storageProvider.GetFile(mediaPath);
            var stream = file.OpenRead();
            return File(stream, file.GetFileType());
        }
    }
}