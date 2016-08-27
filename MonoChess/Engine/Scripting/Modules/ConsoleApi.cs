using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Scripting.Modules
{
    public class ConsoleApi
    {
        private static ConsoleApi _instance = null;
        public static ConsoleApi Instance
        {
            get
            {
                if (_instance == null) _instance = new ConsoleApi();
                return _instance;
            }
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
