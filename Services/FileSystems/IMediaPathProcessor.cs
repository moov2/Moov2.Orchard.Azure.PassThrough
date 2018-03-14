using Orchard;

namespace Moov2.Orchard.Azure.PassThrough.Services.FileSystems
{
    public interface IMediaPathProcessor : IDependency
    {
        string CleanPath(string mediaPath);
    }
}
