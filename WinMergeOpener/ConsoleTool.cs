using System;

namespace WinMergeOpener
{
    public static class ConsoleTool
    {
        public static void WriteInfo(string header, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(header + ":");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void WriteError(string header, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(header + ":");
            Console.ResetColor();
            Console.WriteLine(message);
        }


        public static void WriteErrorAll(string header,Exception err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(header + ":");
            Console.ResetColor();


            var writeError = err;

            while (writeError != null)
            {
                Console.WriteLine(writeError.Message);
                writeError = writeError.InnerException;
            }


         //   Console.WriteLine(message);
        }

    }
}
