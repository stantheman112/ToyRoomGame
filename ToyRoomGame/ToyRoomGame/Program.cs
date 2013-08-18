using System;

namespace Toyroom
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameManager gm = new Game1())
            {
               // gm.Run();
            }
        }
    }
#endif
}

