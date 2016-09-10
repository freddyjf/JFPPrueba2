using System;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace JFP.Core.Loggin
{

    public enum eTypeLog
    {
        Info,
        Warning,
        Error
    }

    public class LoggingManager : ILogging
    {
        private static readonly ILog Log = LogManager.GetLogger("Precedente");
        private static bool LoggingSetup;
        public LoggingManager()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var patternLayout = new PatternLayout
            {
                ConversionPattern =
                    "%date\t%-5level\t%logger\t%aspnet-session{username}\t%aspnet-session{token}\t%message%newline"
            };

            //patternLayout.Header
            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender
            {
                AppendToFile = true,
                File = Config.Config.LlaveString("FileLog"),
                Layout = patternLayout,
                CountDirection = -1,
                MaxSizeRollBackups = 10,
                MaximumFileSize = Config.Config.LlaveString("SizeLog"),
                RollingStyle = RollingFileAppender.RollingMode.Date,
                StaticLogFileName = false,
                DatePattern = "-yyyy-MM-dd-tt",
                PreserveLogFileNameExtension = true,
                LockingModel = new FileAppender.MinimalLock()
            };

            //TO-DO: Debe venir del xdp
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);


            var memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            LoggingSetup = true;
        }

        /// <summary>
        /// Escribir un texto o un texto con formato como Información 
        /// </summary>
        /// <param name="message"></param>
        public void writeLog(params string[] message)
        {

            if (message == null
                || !message.Any()
                || message.All(x => string.IsNullOrWhiteSpace(x)))
                return;

            if (message.Length == 1)
                writeLog(eTypeLog.Info, message[0], new Exception(message[0]));
            else
                writeLog(eTypeLog.Info, string.Format(message[0], message.Skip(1).ToArray()), null);
        }

        public void writeLog(eTypeLog type, object message)
        {
            var message1 = message == null ? "No message included" : message.ToString();

            writeLog(eTypeLog.Info, message, new Exception(message1));
        }

        public void writeLog(eTypeLog type, object message, Exception exception)
        {

            switch (type)
            {
                case eTypeLog.Error:
                    if (Log.IsErrorEnabled)
                    {
                        var excepMsg = string.Format("{0} EXCEPTION: {1} STACK: {2}",
                            message,
                            exception.Message,
                            exception.StackTrace.Replace("\r\n", " "));
                        Log.Error(excepMsg);
                    }
                    break;
                case eTypeLog.Info:
                    if (Log.IsInfoEnabled)
                        Log.Info(message);
                    break;
            }


        }
    }
}
