using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaInterface;
using MonoChess.Engine.Scripting.Modules;

namespace MonoChess.Engine
{
    public static class LuaMaster
    {
        public static Lua State;
        public static void RegisterApi()
        {
            State = new Lua();
            State["Scripting"] = LuaApi.Instance;
            State["Notation"] = Notation.Instance;
            State["Console"] = ConsoleApi.Instance;
        }
    }
}
