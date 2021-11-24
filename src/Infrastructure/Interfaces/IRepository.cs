using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
	public interface IRepository<T> where T : BaseEntity, IAggregateRoot
	{
		DbContext DbContext { get; }
		DbSet<T> DbSet { get; }

		T GetById(int id);
		T GetSingleBySpec(ISpecification<T> spec);
		IEnumerable<T> ListAll();
		IEnumerable<T> List(ISpecification<T> spec);


		T Add(T entity);
		void Update(T entity);
		void Delete(T entity);


		T Get(Expression<Func<T, bool>> criteria);
		List<T> GetMany(Expression<Func<T, bool>> criteria);

		void AddRange(IEnumerable<T> entityList);
		void UpdateRange(IEnumerable<T> entityList);
		void DeleteRange(IEnumerable<T> entityList);


		void Update(T existingEntity, T model);
		void SyncList(IList<T> existingList, IList<T> latestList);

		

	}
}
