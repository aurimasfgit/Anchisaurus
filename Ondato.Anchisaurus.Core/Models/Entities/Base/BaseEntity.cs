namespace Ondato.Anchisaurus.Core.Models.Entities.Base
{
    public interface IBaseEntity<TIdentificator>
    {
        TIdentificator Id { get; set; }
    }

    public abstract class BaseEntity<TIdentificator> : IBaseEntity<TIdentificator>
    {
        public TIdentificator Id { get; set; }
    }
}