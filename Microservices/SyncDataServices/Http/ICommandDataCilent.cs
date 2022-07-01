using Microservices.Dtos;

namespace PlatformsService.SyncDataServices.Http
{
    public interface ICommandDataCilent
    {
        Task SendPlatfromToCommand(PlatformReadDto platformReadDto);

    }
}
