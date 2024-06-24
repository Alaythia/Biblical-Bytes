namespace BiblicalBytes.Contracts;

public interface ILogger
{
    /// <summary>
    /// Write a log event with the verbose level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    void Verbose(string messageTemplate);

    /// <summary>
    /// Write a log event with the verbose level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    void Verbose<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the verbose level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the verbose level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the verbose level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    void Verbose(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the verbose level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    void Verbose(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the verbose level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    void Verbose<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the verbose level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    void Verbose<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the verbose level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    void Verbose<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the verbose level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    void Verbose(Exception? exception, string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the debug level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    void Debug(string messageTemplate);

    /// <summary>
    /// Write a log event with the debug level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    void Debug<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the debug level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the debug level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the debug level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    void Debug(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the debug level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    void Debug(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the debug level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    void Debug<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the debug level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    void Debug<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the debug level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    void Debug<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the debug level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    void Debug(Exception? exception, string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the information level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information(string messageTemplate);

    /// <summary>
    /// Write a log event with the information level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the information level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the information level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the information level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the information level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the information level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the information level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the information level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the information level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    void Information(Exception? exception, string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the warning level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning(string messageTemplate);

    /// <summary>
    /// Write a log event with the warning level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the warning level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the warning level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the warning level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the warning level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the warning level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the warning level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the warning level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the warning level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    void Warning(Exception? exception, string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the error level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error(string messageTemplate);

    /// <summary>
    /// Write a log event with the error level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the error level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the error level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the error level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the error level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the error level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the error level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the error level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the error level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    void Error(Exception? exception, string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the fatal level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    void Fatal(string messageTemplate);

    /// <summary>
    /// Write a log event with the fatal level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    void Fatal<T>(string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the fatal level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the fatal level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the fatal level.
    /// </summary>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    void Fatal(string messageTemplate, params object?[]? propertyValues);

    /// <summary>
    /// Write a log event with the fatal level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    void Fatal(Exception? exception, string messageTemplate);

    /// <summary>
    /// Write a log event with the fatal level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    void Fatal<T>(Exception? exception, string messageTemplate, T propertyValue);

    /// <summary>
    /// Write a log event with the fatal level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    void Fatal<T0, T1>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

    /// <summary>
    /// Write a log event with the fatal level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
    /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    void Fatal<T0, T1, T2>(Exception? exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

    /// <summary>
    /// Write a log event with the fatal level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="messageTemplate">Message template describing the event.</param>
    /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    void Fatal(Exception? exception, string messageTemplate, params object?[]? propertyValues);
}