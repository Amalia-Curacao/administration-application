namespace Creative.Database.Data;

public class InMemoryOptions : DatabaseContextOptions
{
    public override DatabaseSrc DatabaseSrc => DatabaseSrc.InMemory;
}
