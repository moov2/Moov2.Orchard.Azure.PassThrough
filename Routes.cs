using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moov2.Orchard.Azure.PassThrough
{
    [OrchardFeature(Constants.PassThroughMediaFeatureName)]
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Priority = 1,
                    Route = new Route(
                        "SecureMedia/{*mediaPath}",
                        new RouteValueDictionary {
                                                    {"area", "Moov2.Orchard.Azure.PassThrough"},
                                                    {"controller", "Media"},
                                                    {"action", "Index"}
                                                },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                                                    {"area", "Moov2.Orchard.Azure.PassThrough"}
                                                },
                        new MvcRouteHandler())
                }
            };
        }
    }
}