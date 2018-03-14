using Orchard.Azure.Services.Environment.Configuration;
using Orchard.Azure.Services.FileSystems.Media;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;
using Orchard.FileSystems.Media;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace Moov2.Orchard.Azure.PassThrough.Services.FileSystems.Media
{
    [OrchardFeature(Constants.PassThroughMediaFeatureName)]
    [OrchardSuppressDependency("Orchard.Azure.Services.FileSystems.Media.AzureBlobStorageProvider")]
    public class SecuredAzureBlobStorageProvider : AzureBlobStorageProvider, IStorageProvider
    {
        public const string SECURE_MEDIA_PATH = "SecureMedia";

        private readonly string _publicPath;

        public SecuredAzureBlobStorageProvider(ShellSettings shellSettings, IMimeTypeProvider mimeTypeProvider, IPlatformConfigurationAccessor pca)
            : this(pca.GetSetting(global::Orchard.Azure.Constants.MediaStorageStorageConnectionStringSettingName, shellSettings.Name, null),
                   global::Orchard.Azure.Constants.MediaStorageContainerName,
                   pca.GetSetting(global::Orchard.Azure.Constants.MediaStorageRootFolderPathSettingName, shellSettings.Name, null) ?? shellSettings.Name,
                   mimeTypeProvider,
                   pca.GetSetting(global::Orchard.Azure.Constants.MediaStoragePublicHostName, shellSettings.Name, null))
        {
            var mediaPath = HostingEnvironment.IsHosted
                                ? HostingEnvironment.MapPath("~/" + SECURE_MEDIA_PATH + "/") ?? ""
                                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SECURE_MEDIA_PATH);

            var appPath = "";
            if (HostingEnvironment.IsHosted)
            {
                appPath = HostingEnvironment.ApplicationVirtualPath;
            }
            if (!appPath.EndsWith("/"))
                appPath = appPath + '/';
            if (!appPath.StartsWith("/"))
                appPath = '/' + appPath;

            _publicPath = appPath + SECURE_MEDIA_PATH + "/" + shellSettings.Name + "/";
        }

        public SecuredAzureBlobStorageProvider(string storageConnectionString, string containerName, string rootFolderPath, IMimeTypeProvider mimeTypeProvider, string publicHostName)
            : base(storageConnectionString, containerName, rootFolderPath, mimeTypeProvider, publicHostName)
        {
        }

        public new string GetPublicUrl(string path)
        {
            path = ConvertToRelativeUriPath(path);
            return string.IsNullOrEmpty(path) ? _publicPath : Path.Combine(_publicPath, path).Replace(Path.DirectorySeparatorChar, '/').Replace(" ", "%20");
        }

        public new string GetStoragePath(string url)
        {
            // If the underlying azure file system isn't initialized this will be null
            if (_absoluteRoot != null)
            {
                var baseResult = base.GetStoragePath(url);
                if (baseResult != null)
                {
                    return baseResult;
                }
            }
            if (url != null && url.StartsWith(_publicPath, StringComparison.OrdinalIgnoreCase))
            {
                return HttpUtility.UrlDecode(url.Substring(Combine(_publicPath, "/").Length));
            }
            if (url != null && url.StartsWith(_root, StringComparison.OrdinalIgnoreCase))
            {
                return HttpUtility.UrlDecode(url.Substring(Combine(_root, "/").Length));
            }

            return null;
        }

        private static string ConvertToRelativeUriPath(string path)
        {
            var newPath = path.Replace(@"\", "/");

            if (newPath.StartsWith("/") || newPath.StartsWith("http://") || newPath.StartsWith("https://"))
            {
                throw new ArgumentException("Path must be relative");
            }

            return newPath;
        }
    }
}