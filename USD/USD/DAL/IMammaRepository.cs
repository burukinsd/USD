using System.Collections.Generic;
using LiteDB;
using USD.MammaModels;

namespace USD.DAL
{
    public interface IMammaRepository
    {
        ObjectId Add(MammaModel item);
        MammaModel GetById(ObjectId id);
        void Delete(ObjectId id);
        void Update(MammaModel item);
        List<MammaModel> GetAll();
    }
}