using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();
        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(int platformId);

        //Commands
        IEnumerable<Command> GetAllCommands();
        Command GetCommand(int platformId, int commandId);
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        void CreateCommand(int platformId, Command command);

    }
}