using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
	public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
	{
		Task<T> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> ListAllAsync();
		Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<int> CountAsync(ISpecification<T> spec);

		Task UpdateAsync(T existingEntity, T model);

	}

}
