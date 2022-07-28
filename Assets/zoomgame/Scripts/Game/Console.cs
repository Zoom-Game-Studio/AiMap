using System;
using QFramework;
using UniRx.Diagnostics;
using UnityEngine;
using Logger = UniRx.Diagnostics.Logger;

namespace WeiXiang
{
    public static class Console
    {
        private static UniRx.Diagnostics.Logger _Logger;

        public static Logger Logger
        {
            get
            {
                if (_Logger is null)
                {
                    _Logger = new Logger("Wei_Xiang_Console");
                    ObservableLogger.Listener.LogToUnityDebug();
                }
                return _Logger;
            }
        }

        /// <summary>Output LogType.Log but only enables isDebugBuild</summary>
        public static void Debug(object message, UnityEngine.Object context = null)
        {
            Logger.Debug(message,context);
        }
        
        public static void Log(object message, UnityEngine.Object context = null)
        {
            Logger.Log(message,context);
        }

        public static void Warning(object message, UnityEngine.Object context = null)
        {
            Logger.Warning(message,context);
        }

        public static void Error(object message, UnityEngine.Object context = null)
        {
            Logger.Error(message,context);
        }

        public static  void Exception(Exception exception, UnityEngine.Object context = null)
        {
            Logger.Error(exception,context);
        }

        /// <summary>Publish raw LogEntry.</summary>
        public static void Raw(LogEntry logEntry)
        {
            Logger.Raw(logEntry);
        }



    }
}