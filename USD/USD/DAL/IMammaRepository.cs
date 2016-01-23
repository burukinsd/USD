using LiteDB;

namespace USD.DAL
{
    public interface IMammaRepository
    {
        void Add(MammaModel.MammaModel item);
        MammaModel.MammaModel GetById(ObjectId id);
        void Delete(ObjectId id);
        void Update(MammaModel.MammaModel item);
    }
}