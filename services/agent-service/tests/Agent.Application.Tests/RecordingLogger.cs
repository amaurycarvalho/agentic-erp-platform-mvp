using Microsoft.Extensions.Logging;

namespace Agent.Application.Tests;

public sealed class RecordingLogger<T> : ILogger<T>
{
    private readonly List<RecordedLog> _entries = new();

    public IReadOnlyList<RecordedLog> Entries => _entries;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var template = string.Empty;
        var args = new List<object?>();
        if (state is IEnumerable<KeyValuePair<string, object?>> values)
        {
            foreach (var value in values)
            {
                if (value.Key.Trim('{', '}') == "OriginalFormat")
                {
                    template = value.Value?.ToString() ?? string.Empty;
                }
                else
                {
                    args.Add(value.Value);
                }
            }
        }

        _entries.Add(new RecordedLog(logLevel, template, args, formatter(state, exception)));
    }
}

public sealed record RecordedLog(
    LogLevel Level,
    string Template,
    IReadOnlyList<object?> Args,
    string Message);
