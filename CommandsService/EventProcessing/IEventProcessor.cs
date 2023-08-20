namespace CommandsService.EventProcessor
{
    public interface IEventProcessor
    {
        void ProcessMessage(string message);
    }
}