using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamLibraryUpdateBehaviorChanger
{
    class VDFHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> GetKeyPair(IList<string> data, string keyName)
        {
            return from line in data where line.Contains(keyName) select line.Split('\"', '\"') into output select new KeyValuePair<string, string>(output[1], output[3]);
        }
    }
}
