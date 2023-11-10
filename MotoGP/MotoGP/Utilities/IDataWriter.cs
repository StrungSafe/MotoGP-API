namespace MotoGP.Utilities;

public interface IDataWriter
{
    Task Write<T>(string filePath, T data, CancellationToken token = default);
}