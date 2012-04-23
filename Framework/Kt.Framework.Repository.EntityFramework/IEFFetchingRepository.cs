using System.Data.Objects.DataClasses;
namespace Kt.Framework.Repository.Data.EntityFramework
{
    public interface IEFFetchingRepository<TEntity, TFetch> : IRepository<TEntity> where TEntity : EntityObject
    {
        EFRepository<TEntity> RootRepository { get; }

        string FetchingPath { get; }
    }
}