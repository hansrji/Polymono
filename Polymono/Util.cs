using System;
using System.Threading;

namespace Polymono
{
    public class Util
    {
        public static string NL => Environment.NewLine;
        public static string Tab => "\t";

        public static string ThreadID => 
            $"[{Thread.CurrentThread.ManagedThreadId}:{(string.IsNullOrWhiteSpace(Thread.CurrentThread.Name) ? "Pool" : Thread.CurrentThread.Name)}]";
    }
}
