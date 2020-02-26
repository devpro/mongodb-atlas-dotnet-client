using System.Threading.Tasks;

namespace MongoDb.Atlas.Client.ConsoleApp.Tasks
{
    public interface IConsoleTask
    {
        Task<string> ExecuteAsync(CommandLineOptions options);
    }
}
