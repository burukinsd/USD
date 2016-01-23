using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using USD.MammaModels;
using USD.Properties;

namespace USD.DAL
{
    class LiteDbWraper : IDbWraper
    {
        readonly Dictionary<Type, string> _collectionsDictionary = new Dictionary<Type, string>()
        {
            { typeof(MammaModel), "screenings"}
        }; 

        public ObjectId Add<T>(T item) where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof (T)]);

               return col.Insert(item);
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

        public IEnumerable<T> GetAll<T>() where T : new()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
                var col = db.GetCollection<T>(_collectionsDictionary[typeof(T)]);

                return col.FindAll().ToList();
            }
        }
    }
}