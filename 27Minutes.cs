using System;

namespace _27Minutes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Manager game = new Manager())
            {
                game.Run();
            }
        }
    }
}

