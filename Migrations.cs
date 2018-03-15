using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using System.Linq;

namespace Moov2.Orchard.Azure.PassThrough
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition("SecureMediaPart", p =>
                p.WithDescription("Used with the Secure Media feature to store the secure status of the attached item")
            );

            var mediaTypes = ContentDefinitionManager.ListTypeDefinitions().Where(x => x.Settings.ContainsKey("Stereotype") && "Media".Equals(x.Settings["Stereotype"], System.StringComparison.OrdinalIgnoreCase));

            foreach (var mediaType in mediaTypes)
            {
                if ((mediaType.DisplayName?.Contains("External") ?? false) || (mediaType.Name?.Contains("External") ?? false))
                    continue;
                ContentDefinitionManager.AlterTypeDefinition(mediaType.Name, t =>
                    t.WithPart("SecureMediaPart")
                );
            }

            return 1;
        }
    }
}