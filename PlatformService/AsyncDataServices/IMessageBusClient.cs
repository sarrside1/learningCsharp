using PlatformService.DTOs;

namespace PlatformServices.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDtoPlatform);
    }
}