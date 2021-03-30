using LiteDB;
using Ondato.Anchisaurus.Core.Models.Entities.Base;
using Ondato.Anchisaurus.Core.Models.Exceptions;
using Ondato.Anchisaurus.Dal.Context;
using System;

namespace Ondato.Anchisaurus.Dal.Repositories.Base
{
    public interface IBaseRepository<TEntity>
        where TEntity : Entity
    {
        TEntity GetById(Guid id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(Guid id);
    }

    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : Entity
    {
        private readonly LiteDbContext context;

        public BaseRepository(LiteDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.context = context;
        }

        protected ILiteCollection<TEntity> Collection
        {
            get { return context.Database.GetCollection<TEntity>(); }
        }

        public TEntity GetById(Guid id)
        {
            return Collection.FindById(id);
        }

        public void Add(TEntity entity)
        {
            Collection.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            if (!Collection.Update(entity))
                throw new DocumentNotFoundException($"Document of type {typeof(TEntity)} with ID {entity.Id} was not found in collection");
        }

        public void Delete(Guid id)
        {
            if (!Collection.Delete(id))
                throw new DocumentNotFoundException($"Failed to delete document of type {typeof(TEntity)} with ID {id} from collection");
        }
    }
}