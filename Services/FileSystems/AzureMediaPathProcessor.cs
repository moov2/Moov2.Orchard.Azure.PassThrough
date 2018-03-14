using Orchard.Azure.Services.Environment.Configuration;
using Orchard.Environment.Configuration;
using System;

namespace Moov2.Orchard.Azure.PassThrough.Services.FileSystems
{
    public class AzureMediaPathProcessor : IMediaPathProcessor
    {
        #region Dependencies
        private readonly string _root;
        #endregion

        #region Constructor
        public AzureMediaPathProcessor(ShellSettings shellSettings, IPlatformConfigurationAccessor pca)
        {
            var root = pca.GetSetting(global::Orchard.Azure.Constants.MediaStorageRootFolderPathSettingName, shellSettings.Name, null) ?? shellSettings.Name;
            _root = String.IsNullOrEmpty(root) ? "" : root + "/";
        }
        #endregion

        #region Implementation
        public string CleanPath(string path)
        {
            if (path.StartsWith(_root, StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(_root.Length);
            }
            return path;
        }
        #endregion
    }
}