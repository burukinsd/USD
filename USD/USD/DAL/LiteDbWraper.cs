using System;
using System.Collections.Generic;
using LiteDB;
using USD.Properties;

namespace USD.DAL
{
    class LiteDbWraper : ILiteDbWraper
    {
        readonly Dictionary<Type, string> _collectionsDictionary = new Dictionary<Type, string>()
        {
            { typeof(MammaModel.MammaModel), "screenings"}
        }; 

        public void Add<T>(T item) where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof (T)]);

                col.Insert(item);
            }
        }

        public T GetById<T>(ObjectId id) where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof(T)]);

                return col.FindById(id);
            }
        }

        public void Delete<T>(ObjectId id) where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof(T)]);

                col.Delete(id);
            }
        }

        public void Update<T>(T item) where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof(T)]);

                col.Update(item);
            }
        }
    }
}