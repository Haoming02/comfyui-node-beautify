using Newtonsoft.Json.Linq;
using static NodeBeautify.Configs;

namespace NodeBeautify
{
    internal static class Functions
    {
        private static float Round(JToken value) => (float)value - ((float)value % GRID_SIZE);

        internal static void FixNodes(JObject data)
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

        internal static void FixGroups(JObject data)
        {
            var groups = (JArray)data["groups"];

            foreach (JObject group in groups)
            {
                JArray bound = (JArray)group["bounding"];
                for (int edge = 0; edge < 4; edge++)
                    bound[edge] = Round(bound[edge]);
            }
        }

        internal static void SortNode(JObject data)
        {
            var nodes = (JArray)data["nodes"];
            var sortedNodes = nodes.OrderBy(obj => (int)obj["id"]).ToArray();
            (data["nodes"] as JArray).ReplaceAll(sortedNodes);
        }
    }
}
