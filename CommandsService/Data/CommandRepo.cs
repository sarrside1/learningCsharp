using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        void ICommandRepo.CreateCommand(int platformId, Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        void ICommandRepo.CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }else
            {
                _context.Platforms.Add(platform);
            }
        }

        IEnumerable<Command> ICommandRepo.GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        IEnumerable<Platform> ICommandRepo.GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        Command ICommandRepo.GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        IEnumerable<Command> ICommandRepo.GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(p => p.PlatformId == platformId)
                .OrderBy(p => p.Platform.Name );
        }

        bool ICommandRepo.PlatformExists(int platformId)
        {
            return _context.Platforms.Any(platform => platform.Id == platformId);
        }

        bool ICommandRepo.SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}