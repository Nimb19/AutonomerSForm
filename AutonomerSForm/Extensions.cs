using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutonomerSForm
{
    public static class Extensions
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        public static string ReadFile(string pathToFile)
        {
            using (var sr = new StreamReader(pathToFile, Encoding))
                return sr.ReadToEnd();
        }

        public static void WriteToFile(string text, string pathToFile, bool append = false)
        {
            using (var sw = new StreamWriter(pathToFile, append, Encoding))
                sw.Write(text);
        }

        public static T DeserializeFile<T>(string pathToFile)
        {
            var fileText = ReadFile(pathToFile);
            return JsonConvert.DeserializeObject<T>(fileText);
        }

        public static void SerializeToFile<T>(T obj, string pathToFile)
        {
            var text = JsonConvert.SerializeObject(obj, Formatting.Indented);
            WriteToFile(text, pathToFile, append: false);
        }

        /// <summary> 
        ///     Стартовать процесс 
        /// </summary>
        /// <returns> ExitCode </returns>
        public static int StartProcess(ILogger logger, string args, string workingDirectory
            , string fileName = "cmd.exe ", bool useAdminRights = false)
        {
            var internalEncoding = /*encoding ??*/ Encoding.UTF8;
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false, // The Process object must have the UseShellExecute property set to false in order to redirect IO streams
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = internalEncoding,
                StandardErrorEncoding = internalEncoding,
            };
            if (!string.IsNullOrWhiteSpace(workingDirectory))
                processStartInfo.WorkingDirectory = workingDirectory;
            if (useAdminRights)
                processStartInfo.Verb = "runas";

            var process = new Process() { StartInfo = processStartInfo };

            process.OutputDataReceived += (s, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) logger.WriteLine($"{e.Data}"); };
            process.ErrorDataReceived += (s, e) => { if (!string.IsNullOrWhiteSpace(e.Data)) logger.WriteLine($"[ERR] {e.Data}"); };

            logger.WriteLine("Начало чтения консоли дочернего процесса: ");

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            var exitCode = process.ExitCode;
            process.Close();

            if (exitCode != 0)
            {
                logger.Error($"Процесс завершился с кодом {exitCode}.{Environment.NewLine}\tАргументы: {args}");
                return exitCode;
            }

            logger.WriteLine($"Конец чтения консоли дочернего процесса. Код выхода: {exitCode}");

            return exitCode;
        }
    }
}
