using System.Collections.Generic;

namespace LogParser.Interfaces
{
    public interface ILiteDbService
    {
        int WriteBulk<T>(string collectionName, List<T> list);
    }
}