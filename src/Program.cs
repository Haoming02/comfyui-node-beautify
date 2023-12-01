using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NodeBeautify
{
    internal class Program
    {
        /// <summary>
        /// Round to the cloest multiples of this value
        /// </summary>
        private const int GRID_SIZE = 20;

        /// <summary>
        /// NodeType, (Width, Height)
        /// </summary>
        private static readonly Dictionary<string, (float, float)> FIXED_SIZE = new(){
            {
                "CLIPTextEncode", (400.0f, 150.0f)
            }
        };

        private static bool hasError;
        static void Main(string[] args)
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

        private static float Round(JToken value)
        {
            return (float)value - ((float)value % GRID_SIZE);
        }

        private static void FixNodes(JObject data)
        {
            var nodes = (JArray)data["nodes"];

            // Doing try-catch cause somehow the value can be either array or dictionary...
            foreach (JObject node in nodes)
            {
                try
                {
                    JArray pos = (JArray)node["pos"];
                    pos[0] = Round(pos[0]);
                    pos[1] = Round(pos[1]);
                }
                catch (InvalidCastException)
                {
                    JObject pos = (JObject)node["pos"];
                    pos["0"] = Round(pos["0"]);
                    pos["1"] = Round(pos["1"]);
                }

                string key = (string)node["type"];

                try
                {
                    JObject size = (JObject)node["size"];

                    if (FIXED_SIZE.ContainsKey(key))
                    {
                        size["0"] = FIXED_SIZE[key].Item1;
                        size["1"] = FIXED_SIZE[key].Item2;
                    }
                    else
                    {
                        size["0"] = Round(size["0"]);
                        size["1"] = Round(size["1"]);
                    }
                }
                catch (InvalidCastException)
                {
                    JArray size = (JArray)node["size"];
                    if (FIXED_SIZE.ContainsKey(key))
                    {
                        size[0] = FIXED_SIZE[key].Item1;
                        size[1] = FIXED_SIZE[key].Item2;
                    }
                    else
                    {
                        size[0] = Round(size[0]);
                        size[1] = Round(size[0]);
                    }
                }
            }
        }

        private static void FixGroups(JObject data)
        {
            var groups = (JArray)data["groups"];

            foreach (JObject group in groups)
            {
                JArray bound = (JArray)group["bounding"];
                for (int edge = 0; edge < 4; edge++)
                    bound[edge] = Round(bound[edge]);
            }
        }

        private static void Process(string path)
        {
            string jsonContent = File.ReadAllText(path).Trim();
            var data = JsonConvert.DeserializeObject<JObject>(jsonContent);

            try
            {
                FixNodes(data);
                FixGroups(data);
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
