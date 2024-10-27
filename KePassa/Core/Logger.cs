using System.Text;

namespace SecretStore.Core;

public class Logger {
    public void Info(FormattableString message, Exception? exception = null) {
        Log(LogLevel.Info, message, exception);
    }

    public void Warn(FormattableString message, Exception? exception = null) {
        Log(LogLevel.Warning, message, exception);
    }

    public void Error(FormattableString message, Exception? exception = null) {
        Log(LogLevel.Error, message, exception);
    }

    public void Log(LogLevel level, FormattableString? message, Exception? exception = null) {
        var msg = new LogMessage(level, DateTime.UtcNow, message, exception);
        var formatted = FormatMessage(msg);
        Console.WriteLine(formatted);
    }

    public static string FormatMessage(LogMessage message) {
        var result = new StringBuilder();
        result.Append($"[{message.Timestamp:dd.MM.yyyy HH:mm:ss.fff}] {GetLogLevelString(message.Level)}");
        if (message.Message is not null) {
            result.Append($" {message.Message}");
        }

        if (message.Exception is not null) {
            result.Append($" {message.Exception}");
        }

        return result.ToString();
    }

    private static string GetLogLevelString(LogLevel level) => level switch {
        LogLevel.Trace => "TRC",
        LogLevel.Debug => "DBG",
        LogLevel.Info => "INF",
        LogLevel.Warning => "WRN",
        LogLevel.Error => "ERR",
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
    };

    public record LogMessage(LogLevel Level, DateTime Timestamp, FormattableString? Message, Exception? Exception);

    public enum LogLevel {
        Trace,
        Debug,
        Info,
        Warning,
        Error
    }
}
