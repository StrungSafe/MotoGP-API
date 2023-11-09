namespace MotoGP;

public class ThrottlingDelegatingHandler : DelegatingHandler
{
    private readonly SemaphoreSlim semaphore;

    public ThrottlingDelegatingHandler()
    {
        semaphore = new SemaphoreSlim(4, 6);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await semaphore.WaitAsync(cancellationToken);
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }
}