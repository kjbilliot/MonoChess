using Mantin.Controls.Wpf.Notification;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NotificationsExtensions.Toasts;
using NotificationsExtensions;
using System.Xml;

namespace MonoChess.Engine.Rendering
{
    public static class Notifications
    {
        public static void Notify(string s)
        {
            Console.WriteLine("[Notification] " + s);
        }
        
    }
}
