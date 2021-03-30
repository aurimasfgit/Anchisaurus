using Ondato.Anchisaurus.Core.Models.Entities;
using Ondato.Anchisaurus.Dal.Context;
using Ondato.Anchisaurus.Dal.Repositories.Base;

namespace Ondato.Anchisaurus.Dal.Repositories
{
    public interface IKeyValuePairWithExpirationRepository : IBaseRepository<KeyValuePairWithExpiration>
    {
    }

    public class KeyValuePairWithExpirationRepository : BaseRepository<KeyValuePairWithExpiration>, IKeyValuePairWithExpirationRepository
    {
        public KeyValuePairWithExpirationRepository(LiteDbContext context)
            : base(context)
        {
        }
    }
}