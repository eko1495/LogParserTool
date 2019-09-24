namespace LogParser.Interfaces
{
    public interface IConvertService<T> where T : new()
    {
        T ToObject(string line);
    }
}