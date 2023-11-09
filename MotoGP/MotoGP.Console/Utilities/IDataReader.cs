namespace MotoGP.Utilities;

public interface IDataReader
{
    Task<T> Read<T>(string filePath, CancellationToken token = default);
}