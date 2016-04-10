using System.Collections.Generic;
using System.Linq;
using LiteDB;
using USD.MammaModels;

namespace USD.DAL
{
    internal class MammaRepository : IMammaRepository
    {
        private readonly IDbWraper _dbwraper;

        public MammaRepository(IDbWraper dbwraper)
        {
            _dbwraper = dbwraper;
        }

        public ObjectId Add(MammaModel item)
        {
            return _dbwraper.Add(item);
        }

        public MammaModel GetById(ObjectId id)
        {
            return _dbwraper.GetById<MammaModel>(id);
        }

        public void Delete(ObjectId id)
        {
            _dbwraper.Delete<MammaModel>(id);
        }

        public void Update(MammaModel item)
        {
            _dbwraper.Update(item);
        }

        public List<MammaModel> GetAll()
        {
            return _dbwraper.GetAll<MammaModel>().ToList();
        }
    }
}