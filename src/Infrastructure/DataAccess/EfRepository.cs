using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using Infrastructure.Entities;

namespace Infrastructure.DataAccess
{
	public class EfRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		public DbSet<T> DbSet => _dbSet;
		public DbContext DbContext => _dbContext;

		public EfRepository(DbContext dbContext)
		{
			this._dbContext = dbContext;
			this._dbSet = dbContext.Set<T>();
		}

		#region IAsyncRepository

		public virtual async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

		public async Task<IReadOnlyList<T>> ListAllAsync() => await _dbSet.ToListAsync();

		public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec) => await ApplySpecification(spec).ToListAsync();

		public async Task<int> CountAsync(ISpecification<T> spec) => await ApplySpecification(spec).CountAsync();

		public async Task<T> AddAsync(T entity)
		{
			_dbSet.Add(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(T existingEntity, T model)
		{
			_dbContext.Entry(existingEntity).CurrentValues.SetValues(model);
			await UpdateAsync(existingEntity);
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		private IQueryable<T> ApplySpecification(ISpecification<T> spec) => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);

		#endregion


		#region IRepository

		public T GetById(int id) => _dbSet.Find(id);

		public T GetSingleBySpec(ISpecification<T> spec) => List(spec).FirstOrDefault();

		public IEnumerable<T> ListAll() => _dbSet.AsEnumerable();

		public IEnumerable<T> List(ISpecification<T> spec) => ApplySpecification(spec).AsEnumerable();
		
		public T Add(T entity)
		{
			_dbSet.Add(entity);
			_dbContext.SaveChanges();

			return entity;
		}

		public void Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}

		public void Update(T existingEntity, T model)
		{
			_dbContext.Entry(existingEntity).CurrentValues.SetValues(model);
			Update(existingEntity);
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
			_dbContext.SaveChanges();
		}

		public T Get(Expression<Func<T, bool>> criteria) => _dbSet.Where(criteria).FirstOrDefault();

		public List<T> GetMany(Expression<Func<T, bool>> criteria) => _dbSet.Where(criteria).ToList();

		public void AddRange(IEnumerable<T> entityList)
		{
			_dbSet.AddRange(entityList);
			_dbContext.SaveChanges();
		}

		public void UpdateRange(IEnumerable<T> entityList)
		{
			_dbSet.UpdateRange(entityList);
			_dbContext.SaveChanges();
		}

		public void DeleteRange(IEnumerable<T> entityList)
		{
			_dbSet.RemoveRange(entityList);
			_dbContext.SaveChanges();
		}

		public void SyncList(IList<T> existingList, IList<T> latestList)
		{
			foreach (var existingItem in existingList)
			{
				if (!latestList.Any(item => item.Id == existingItem.Id))
				{
					_dbSet.Remove(existingItem);
				}
			}

			foreach (var latestItem in latestList)
			{
				var existingItem = existingList.Where(item => item.Id == latestItem.Id).FirstOrDefault();

				if (existingItem != null) _dbContext.Entry(existingItem).CurrentValues.SetValues(latestItem);
				else _dbSet.Add(latestItem);

			}

			_dbContext.SaveChanges();
		}


		#endregion
	}
}
