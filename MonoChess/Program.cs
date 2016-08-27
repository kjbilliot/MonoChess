using MonoChess.Engine;
using MonoChess.Engine.Chess;
using MonoChess.Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonoChess
{
#if WINDOWS || LINUX

    public static class Program
    {
        public static ChessGame Game;
        public static Thread GameThread;
        public const string SentinelQuitValue = "quit";

        [STAThread]
        static void Main(string[] args)
        {
            InitializeAndLoadLua();
            LaunchGameThread();
            HandleArguments(args);
        }

        private static void HandleArguments(string[] args)
        {
            if (args.Length > 0)
            {
                string filename = args[0];
                if (File.Exists(filename))
                {
                    foreach (string s in File.ReadAllLines(filename))
                    {
                        NotationParser.ParseString(s);
                    }
                }
                else
                {
                    Console.WriteLine("File passed in through command line doesn't exist.");
                }
            }
        }

        private static void LaunchGameThread()
        {
            Game = ChessGame.Instance;
            GameThread = new Thread(Game.Run);
            GameThread.SetApartmentState(ApartmentState.STA);
            GameThread.Start();
        }

        private static void InitializeAndLoadLua()
        {
            LuaMaster.RegisterApi();
            try
            {
                LuaMaster.State.DoFile("GameInit.lua");
            }
            catch (Exception e)
            {
                Console.WriteLine("Lua threw an exception!\n" + e.Message + "\n" + e.StackTrace);
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
                System.Environment.Exit(0);
            }
        }
        
    }
#endif
}
