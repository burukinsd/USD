using System.Collections.Generic;
using LiteDB;

namespace USD.DAL
{
    public interface IDbWraper
    {
        ObjectId Add<T>(T item) where T : new();
        T GetById<T>(ObjectId id) where T : new();
        void Delete<T>(ObjectId id) where T : new();
        void Update<T>(T item) where T : new();
        IEnumerable<T> GetAll<T>() where T : new();
    }
}