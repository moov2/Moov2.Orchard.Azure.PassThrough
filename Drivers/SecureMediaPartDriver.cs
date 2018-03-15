using Moov2.Orchard.Azure.PassThrough.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Security;

namespace Moov2.Orchard.Azure.PassThrough.Drivers
{
    public class SecureMediaPartDriver : ContentPartDriver<SecureMediaPart>
    {
        private readonly IAuthorizer _authorizer;

        public SecureMediaPartDriver(IAuthorizer authorizer)
        {
            _authorizer = authorizer;
        }

        protected override DriverResult Editor(SecureMediaPart part, dynamic shapeHelper)
        {
            if (!_authorizer.Authorize(Permissions.ManageSecureMedia))
                return null;
            return ContentShape("Parts_SecureMedia_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts.SecureMedia.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(SecureMediaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!_authorizer.Authorize(Permissions.ManageSecureMedia))
                return null;
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}