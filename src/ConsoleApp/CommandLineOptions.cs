using CommandLine;

namespace MongoDb.Atlas.Client.ConsoleApp
{
    public class CommandLineOptions
    {
        [Value(0, MetaValue = "Action", Required = true, HelpText = "Action (possible values: \"list\", \"edit\", \"delete\").")]
        public string Action { get; set; }

        [Value(1, MetaValue = "Resource", Required = false, HelpText = "Resource (possible values: \"orgs\", \"projects\", \"events\", \"whitelist\").")]
        public string Resource { get; set; }

        [Value(2, MetaValue = "Id", Required = false, HelpText = "ID.")]
        public string Id { get; set; }

        [Option("publickey", Required = false, HelpText = "Public Api key provided by MongoDB Atlas.")]
        public string PublicKey { get; set; }

        [Option("privatekey", Required = false, HelpText = "Private Api key provided by MongoDB Atlas (KEEP IT SECRET!).")]
        public string PrivateKey { get; set; }

        [Option('p', "project", Required = false, HelpText = "Project id.")]
        public string Project { get; set; }

        [Option('n', "name", Required = false, HelpText = "Name.")]
        public string Name { get; set; }

        [Option('q', "query", Required = false, HelpText = "Information to send back.")]
        public string Query { get; set; }

        [Option("values", Required = false, HelpText = "Data values.")]
        public string Values { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool IsVerbose { get; set; }
    }
}
