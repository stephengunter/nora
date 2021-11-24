using Infrastructure.DataAccess;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess
{
	
	public interface IDefaultRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
	{

	}
	
	public class DefaultRepository<T> : EfRepository<T>, IDefaultRepository<T> where T : BaseEntity, IAggregateRoot
	{
		public DefaultRepository(DefaultContext context) : base(context)
		{

		}
	}
}
