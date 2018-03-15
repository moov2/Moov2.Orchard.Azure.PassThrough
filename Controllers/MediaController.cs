using Moov2.Orchard.Azure.PassThrough.Models;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using Orchard.MediaLibrary.Services;
using Orchard.Security;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Moov2.Orchard.Azure.PassThrough.Controllers
{
    [OrchardFeature(Constants.PassThroughMediaFeatureName)]
    public class MediaController : Controller
    {
        private readonly IAuthorizer _authorizer;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IStorageProvider _storageProvider;

        public MediaController(IAuthorizer authorizer, IMediaLibraryService mediaLibraryService, IStorageProvider storageProvider)
        {
            _authorizer = authorizer;
            _mediaLibraryService = mediaLibraryService;
            _storageProvider = storageProvider;
        }

        public ActionResult Index(string mediaPath)
        {
            mediaPath = _storageProvider.GetStoragePath(mediaPath);
            if (mediaPath == null)
                return HttpNotFound();
            var fileName = Path.GetFileName(mediaPath);
            var directory = mediaPath.Substring(0, mediaPath.IndexOf(fileName));
            directory = directory.TrimEnd('/');
            var medias = _mediaLibraryService
                .GetMediaContentItems(VersionOptions.Published)
                .Where(x => x.FolderPath == directory && x.FileName == fileName)
                .List();
            var isSecure = medias.All(x => x.As<SecureMediaPart>()?.IsSecure ?? true);
            if ((isSecure && !_authorizer.Authorize(Permissions.ViewSecureMedia)) || !_storageProvider.FileExists(mediaPath))
            {
                return HttpNotFound();
            }
            var file = _storageProvider.GetFile(mediaPath);
            var stream = file.OpenRead();
            return File(stream, file.GetFileType());
        }
    }
}