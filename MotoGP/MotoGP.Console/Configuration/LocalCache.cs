namespace MotoGP.Configuration;

public class LocalCache
{
    public Uri Directory { get; set; }

    public bool Enabled { get; set; }

    public bool Overwrite { get; set; } = false;

    public bool OverwriteOnError { get; set; } = true;
}