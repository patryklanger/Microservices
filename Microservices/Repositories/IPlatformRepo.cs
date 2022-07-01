using Microservices.Models;

namespace Microservices.Repositories
{
    public interface IPlatformRepo
    {
        bool SaveChange();

        IEnumerable<Platform> GetAllPlatforms();

        Platform? GetPlatfromById(int id);

        void CreatePlatform(Platform platform);
        }
}
