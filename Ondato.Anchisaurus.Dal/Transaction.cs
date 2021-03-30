using Ondato.Anchisaurus.Dal.Context;
using System;

namespace Ondato.Anchisaurus.Dal
{
    public interface ITransaction
    {
        void Begin();
        void Commit();
        void Rollback();
    }

    public class Transaction : ITransaction
    {
        private readonly LiteDbContext liteDbContext;

        public Transaction(LiteDbContext liteDbContext)
        {
            this.liteDbContext = liteDbContext ?? throw new ArgumentNullException(nameof(liteDbContext));
        }

        public void Begin()
        {
            liteDbContext.Database.BeginTrans();
        }

        public void Commit()
        {
            liteDbContext.Database.Commit();
        }

        public void Rollback()
        {
            liteDbContext.Database.Rollback();
        }
    }
}