using LogParser.Model;
using LogParser.Services;
using System;
using System.IO;

namespace LogParser
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// LogParser
        /// </summary>
        /// <param name="args">Path to log file</param>
        static void Main(string[] args)
        {
            Logger.Info("Start LogParser");

            if (args.Length == 0)
            {
                Console.WriteLine("Missing path to log file.");
                //Console.ReadLine();
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Log file does not exist: {filePath}");
            }


            ProcessLog processLog = new ProcessLog(filePath, new JsonService<LogLine>(), Logger, new ProcessService(Logger));
            try
            {
                Logger.Info($"Start processing: {filePath}");
                processLog.Run();
            }
            catch (FileNotFoundException ex)
            {
                Logger.Error(ex);
                Console.WriteLine(ex.Message);
                //https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499-?redirectedfrom=MSDN
                Environment.Exit(2); //ERROR_FILE_NOT_FOUND
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Error(ex);
                Console.WriteLine(ex.Message);
                Environment.Exit(8);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            Console.WriteLine("Finish");
            //Console.ReadLine();

            Logger.Info("End LogParser");
        }
    }
}


