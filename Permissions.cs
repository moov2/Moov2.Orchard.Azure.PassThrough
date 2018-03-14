using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System.Collections.Generic;

namespace Moov2.Orchard.Azure.PassThrough
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageSecureMedia = new Permission { Description = "Manage Secure Media", Name = "ManageSecureMedia" };
        public static readonly Permission ViewSecureMedia = new Permission { Description = "View Secure Media", Name = "ViewSecureMedia", ImpliedBy = new[] { ManageSecureMedia } };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ManageSecureMedia, ViewSecureMedia };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageSecureMedia, ViewSecureMedia }
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] { ViewSecureMedia }
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new [] { ViewSecureMedia }
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] { ViewSecureMedia }
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] { ViewSecureMedia }
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] { ViewSecureMedia }
                }
            };
        }
    }
}