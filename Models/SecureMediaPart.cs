using Orchard.ContentManagement;

namespace Moov2.Orchard.Azure.PassThrough.Models
{
    public class SecureMediaPart : ContentPart
    {
        public bool IsSecure
        {
            get { return this.Retrieve(x => x.IsSecure, true); }
            set { this.Store(x => x.IsSecure, value); }
        }
    }
}