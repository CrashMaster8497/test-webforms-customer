namespace CustomerLibrary.Repositories
{
    public interface IRepository<TEntity>
    {
        public int? Create(TEntity entity);
        public TEntity? Read(int entityId);
        public bool Update(TEntity entity);
        public bool Delete(int entityId);
    }
}
