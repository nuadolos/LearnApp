using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Helper.Logging
{
    static public class ILoggerExtension
    {
        public static void LogDebugWithContext(this ILogger logger, string message, 
            object[]? messageParams = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogContext.PushProperty("Method", memberName);
            LogContext.PushProperty("SourceFilePath", sourceFilePath);
            LogContext.PushProperty("SourceLineNumber", sourceLineNumber);
            logger.LogDebug(message, messageParams!);
        }

        public static void LogInformationWithContext(this ILogger logger, string message,
            object[]? messageParams = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogContext.PushProperty("Method", memberName);
            LogContext.PushProperty("SourceFilePath", sourceFilePath);
            LogContext.PushProperty("SourceLineNumber", sourceLineNumber);
            logger.LogInformation(message, messageParams!);
        }

        public static void LogWarningWithContext(this ILogger logger, string message,
            object[]? messageParams = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogContext.PushProperty("Method", memberName);
            LogContext.PushProperty("SourceFilePath", sourceFilePath);
            LogContext.PushProperty("SourceLineNumber", sourceLineNumber);
            logger.LogWarning(message, messageParams!);
        }

        public static void LogErrorWithContext(this ILogger logger, Exception exception, string message,
            object[]? messageParams = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogContext.PushProperty("Method", memberName);
            LogContext.PushProperty("SourceFilePath", sourceFilePath);
            LogContext.PushProperty("SourceLineNumber", sourceLineNumber);
            logger.LogError(exception, message, messageParams!);
        }
    }
}
