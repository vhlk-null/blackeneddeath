namespace Library.Application.Services.Import;

public class ImportStatusService
{
    private volatile ImportStatus? _current;

    public ImportStatus? Current => _current;

    public void Start(string bandName) =>
        _current = new ImportStatus(bandName, 0, 0, true, null);

    public void Update(string message, int current, int total) =>
        _current = _current with { Message = message, Current = current, Total = total };

    public void Complete(string message) =>
        _current = _current with { IsRunning = false, Message = message };

    public void Fail(string error) =>
        _current = _current with { IsRunning = false, Message = error };

    public void Clear() => _current = null;
}

public record ImportStatus(
    string BandName,
    int Current,
    int Total,
    bool IsRunning,
    string? Message);
