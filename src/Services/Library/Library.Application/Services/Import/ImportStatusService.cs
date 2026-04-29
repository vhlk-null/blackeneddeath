namespace Library.Application.Services.Import;

public class ImportStatusService
{
    private volatile ImportStatus? _current;
    private CancellationTokenSource? _cts;

    public ImportStatus? Current => _current;

    public CancellationToken Start(string bandName)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
        _current = new ImportStatus(bandName, 0, 0, true, null);
        return _cts.Token;
    }

    public void Update(string message, int current, int total) =>
        _current = _current with { Message = message, Current = current, Total = total };

    public void Complete(string message)
    {
        _current = _current with { IsRunning = false, Message = message };
        _cts?.Dispose();
        _cts = null;
    }

    public void Fail(string error)
    {
        _current = _current with { IsRunning = false, Message = error };
        _cts?.Dispose();
        _cts = null;
    }

    public void Cancel()
    {
        _cts?.Cancel();
        if (_current is not null)
            _current = _current with { IsRunning = false, Message = "Import cancelled." };
    }

    public void Clear() => _current = null;
}

public record ImportStatus(
    string BandName,
    int Current,
    int Total,
    bool IsRunning,
    string? Message);
