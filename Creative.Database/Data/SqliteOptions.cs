namespace Creative.Database.Data;

public class SqliteOptions : DatabaseContextOptions
{
    public override DatabaseSrc DatabaseSrc => DatabaseSrc.Sqlite;
}
