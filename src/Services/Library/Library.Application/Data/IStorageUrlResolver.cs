namespace Library.Application.Data;

public interface IStorageUrlResolver
{
    string? Resolve(string? key);
}
