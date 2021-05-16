using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.Utilities
{
    public class Helper
    {
        public static void beautifyJSon(string str)
        {
            JToken parsedJson = JToken.Parse(str);
            string beautifiedJson = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(beautifiedJson);
        }

    }
}
