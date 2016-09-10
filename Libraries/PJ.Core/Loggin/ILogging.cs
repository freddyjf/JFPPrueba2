using System;

namespace JFP.Core.Loggin
{
   public  interface ILogging
    {
        void writeLog(params string[] message);

        void writeLog(eTypeLog type, object message);

        void writeLog(eTypeLog type, object message, Exception exception);
    }
}
