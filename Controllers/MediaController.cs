using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using Orchard.Security;
using System.Web.Mvc;

namespace Moov2.Orchard.Azure.PassThrough.Controllers
{
    [OrchardFeature(Constants.PassThroughMediaFeatureName)]
    public class MediaController : Controller
    {
        private readonly IAuthorizer _authorizer;
        private readonly IStorageProvider _storageProvider;

        public MediaController(IAuthorizer authorizer, IStorageProvider storageProvider)
        {
            _authorizer = authorizer;
            _storageProvider = storageProvider;
        }

        public ActionResult Index(string mediaPath)
        {
            if (!_authorizer.Authorize(Permissions.ViewSecureMedia))
            {
                return HttpNotFound();
            }
            mediaPath = _storageProvider.GetStoragePath(mediaPath);
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