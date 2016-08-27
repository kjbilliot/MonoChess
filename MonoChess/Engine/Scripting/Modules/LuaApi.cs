using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Scripting.Modules
{
    public class LuaApi
    {
        private static LuaApi _instance = null;
        public static LuaApi Instance
        {
            get
            {
                if (_instance == null) _instance = new LuaApi();
                return _instance;
            }
        }

        public void RunScript(string script)
        {
            LuaMaster.State.DoString(script);
        }

        public void RunFile(string filename)
        {
            if (File.Exists(filename))
            {
                foreach (string s in File.ReadAllLines(filename))
                {
                    RunScript(s);
                }
            }
        }
    }
}
