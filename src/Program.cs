using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NodeBeautify
{
    internal class Program
    {
        private static bool hasError;

        internal static void Main(string[] args)
        {
            hasError = false;

            if (args.Length == 0)
            {
                Console.Write("Path to Workflow: ");
                string input = Console.ReadLine()?.Trim();
                ParseFilePath(input);
            }
            else
            {
                foreach (string filePath in args)
                    ParseFilePath(filePath.Trim());
            }

            if (hasError)
                Pause();
        }

        private static void ParseFilePath(string path)
        {
            if (path.StartsWith('\"') && path.EndsWith('\"'))
                path = path.Trim('"');

            if (!File.Exists(path) || !path.EndsWith(".json"))
            {
                Console.WriteLine($"Invalid File: {path}");
                hasError = true;
                return;
            }

            Process(path);
        }

        private static void Process(string path)
        {
            string jsonContent = File.ReadAllText(path).Trim();
            var data = JsonConvert.DeserializeObject<JObject>(jsonContent);

            try
            {
                Functions.FixNodes(data);
                Functions.FixGroups(data);
                Functions.SortNode(data);
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"File {path} is not a valid workflow!");
                hasError = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong while processing file {path}!\n\n{e}\n");
                hasError = true;
            }
        }

        private static void Pause()
        {
            Console.WriteLine("Press ENTER to Continue...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
