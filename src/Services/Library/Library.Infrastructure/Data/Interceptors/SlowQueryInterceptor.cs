using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Data.Interceptors;

public class SlowQueryInterceptor(ILogger<SlowQueryInterceptor> logger, TimeSpan? threshold = null) : DbCommandInterceptor
{
    private readonly TimeSpan _threshold = threshold ?? TimeSpan.FromMilliseconds(500);

    public override DbDataReader ReaderExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result)
    {
        LogIfSlow(command, eventData.Duration);
        return base.ReaderExecuted(command, eventData, result);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData.Duration);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override int NonQueryExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result)
    {
        LogIfSlow(command, eventData.Duration);
        return base.NonQueryExecuted(command, eventData, result);
    }

    public override ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData.Duration);
        return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }
    
    public override object? ScalarExecuted(
        DbCommand command,
        CommandExecutedEventData eventData,
        object? result)
    {
        LogIfSlow(command, eventData.Duration);
        return base.ScalarExecuted(command, eventData, result);
    }

    public override ValueTask<object?> ScalarExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        object? result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData.Duration);
        return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
    }

    private void LogIfSlow(DbCommand command, TimeSpan duration)
    {
        if (duration < _threshold)
            return;

        List<string> parameters = command.Parameters
            .Cast<DbParameter>()
            .Select(p => $"{p.ParameterName} = {p.Value}")
            .ToList();
        
        logger.LogWarning(
            "[SlowQuery] Query took {Duration}ms (threshold: {Threshold}ms)\n" +
            "SQL:\n{Sql}\n" +
            "Parameters: {Parameters}",
            duration.TotalMilliseconds,
            _threshold.TotalMilliseconds,
            command.CommandText,
            parameters.Count > 0 ? string.Join(", ", parameters) : "none"
        );
    }
}