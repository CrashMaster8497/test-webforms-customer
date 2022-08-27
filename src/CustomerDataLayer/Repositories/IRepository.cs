using System.Collections.Generic;

namespace CustomerLibrary.Repositories
{
    public interface IRepository<TEntity>
    {
        int? Create(TEntity entity);
        TEntity Read(int entityId);
        bool Update(TEntity entity);
        bool Delete(int entityId);
        List<TEntity> ReadAll();
    }
}
