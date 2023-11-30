using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NodeBeautify
{
    internal class Program
    {
        private const int GRID_SIZE = 10;

        private static readonly Dictionary<string, (float, float)> FIXED_SIZE = new(){
            {
                "CLIPTextEncode", (400.0f, 150.0f)
            }
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Drag & Drop a JSON File onto the Executable!");
                Pause();
                return;
            }

            foreach (string filePath in args)
            {
                if (!File.Exists(filePath) || !filePath.EndsWith(".json"))
                {
                    Console.WriteLine($"Invalid File: {filePath}");
                    Pause();
                    continue;
                }

                Process(filePath);
            }
        }

        private static void Process(string path)
        {
            string jsonContent = File.ReadAllText(path);

            try
            {
                var data = JsonConvert.DeserializeObject<JObject>(jsonContent);
                JArray nodes = (JArray)data["nodes"];

                foreach (JObject node in nodes)
                {
                    JArray pos = (JArray)node["pos"];
                    pos[0] = GRID_SIZE * MathF.Round(pos[0].Value<float>() / GRID_SIZE);
                    pos[1] = GRID_SIZE * MathF.Round(pos[1].Value<float>() / GRID_SIZE);

                    try
                    {
                        JObject size = (JObject)node["size"];
                        if (FIXED_SIZE.ContainsKey((string)node["type"]))
                        {
                            size["0"] = FIXED_SIZE[(string)node["type"]].Item1;
                            size["1"] = FIXED_SIZE[(string)node["type"]].Item2;
                        }
                        else
                        {
                            size["0"] = GRID_SIZE * MathF.Round(size["0"].Value<float>() / GRID_SIZE);
                            size["1"] = GRID_SIZE * MathF.Round(size["1"].Value<float>() / GRID_SIZE);
                        }
                    }
                    catch (InvalidCastException)
                    {
                        JArray size = (JArray)node["size"];
                        if (FIXED_SIZE.ContainsKey((string)node["type"]))
                        {
                            size[0] = FIXED_SIZE[(string)node["type"]].Item1;
                            size[1] = FIXED_SIZE[(string)node["type"]].Item2;
                        }
                        else
                        {
                            size[0] = GRID_SIZE * MathF.Round(size[0].Value<float>() / GRID_SIZE);
                            size[1] = GRID_SIZE * MathF.Round(size[1].Value<float>() / GRID_SIZE);
                        }
                    }
                }

                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"File {path} is not a valid workflow!");
                Pause();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong while processing: {path}!\n\n{e}\n");
                Pause();
            }
        }

        private static void Pause()
        {
            Console.WriteLine("Press ENTER to Continue...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
