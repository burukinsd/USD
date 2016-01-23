using LiteDB;

namespace USD.DAL
{
    public interface ILiteDbWraper
    {
        void Add<T>(T item) where T : new();
        T GetById<T>(ObjectId id) where T : new();
        void Delete<T>(ObjectId id) where T : new();
        void Update<T>(T item) where T : new();
    }
}