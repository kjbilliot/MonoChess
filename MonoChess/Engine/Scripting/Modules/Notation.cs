using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Scripting.Modules
{
    public class Notation
    {
        private static Notation _instance = null;
        public static Notation Instance
        {
            get
            {
                if (_instance == null) _instance = new Notation();
                return _instance;
            }
        }
        
        public void ParseString(string notation)
        {
            NotationParser.ParseString(notation);
        }

        public void ParseFile(string filename)
        {
            if (File.Exists(filename))
            {
                foreach (string s in File.ReadAllLines(filename))
                {
                    ParseString(s);
                }
            }
            else
            {
                Console.WriteLine(filename + " does not exist.");
            }
        }
    }
}
