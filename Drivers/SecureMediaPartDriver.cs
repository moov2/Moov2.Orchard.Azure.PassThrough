using Moov2.Orchard.Azure.PassThrough.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Moov2.Orchard.Azure.PassThrough.Drivers
{
    public class SecureMediaPartDriver : ContentPartDriver<SecureMediaPart>
    {
        protected override DriverResult Editor(SecureMediaPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_SecureMedia_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts.SecureMedia.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(SecureMediaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}