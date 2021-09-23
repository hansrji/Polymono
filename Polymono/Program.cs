using OpenTK.Windowing.Common;
using System;
using System.Diagnostics;
using System.Threading;

namespace Polymono
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine($"Program:Main initialised with: {(args.Length > 0 ? string.Join(", ", args) : "No arguments.")}");
            Thread.CurrentThread.Name = "Main";

            //SynchronizationContext prevCtx = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            //Task<string> task = DoOnPool();
            //task.ContinueWith(async delegate
            //{
            //    Debug.WriteLine($"{Util.ThreadID}: ContinueWith init");
            //    await Task.Delay(1000);
            //    Debug.WriteLine($"{Util.ThreadID}: ContinueWith end");
            //}, TaskScheduler.Default);
            //string n = task.GetAwaiter().GetResult();

            //SynchronizationContext.SetSynchronizationContext(prevCtx);


            using PolyWindow game = new(
                new()
                {
                    RenderFrequency = 60d,
                    UpdateFrequency = 60d
                },
                new()
                {
                    API = ContextAPI.OpenGL,
                    APIVersion = new Version(4, 1)
                });
            game.Run();
        }

        //public static async Task Do()
        //{
        //    Debug.WriteLine($"{Util.ThreadID}: Do start.");
        //    string n = await DoOnPool();
        //    Debug.WriteLine($"{Util.ThreadID}: Do end: " + n);
        //}

        //public static async Task<string> DoOnPool()
        //{
        //    Debug.WriteLine($"{Util.ThreadID}: DoOnPool start.");
        //    await Task.Delay(5000);
        //    // Anything after await here is on pool thread.
        //    Debug.WriteLine($"{Util.ThreadID}: DoOnPool end.");
        //    return "Hello";
        //}
    }
}
