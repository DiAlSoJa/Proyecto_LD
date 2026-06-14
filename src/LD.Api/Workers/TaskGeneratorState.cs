namespace LD.Api.Workers;

public sealed class TaskGeneratorState
{
    private volatile bool _isRunning;

    public bool IsRunning => _isRunning;

    public void Start() => _isRunning = true;
    public void Stop()  => _isRunning = false;
}
