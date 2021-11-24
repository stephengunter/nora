using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ApplicationCore.Helpers;

namespace ApplicationCore.Services
{
	public interface IArticlesService
	{
		Task<IEnumerable<Article>> FetchAsync();
		Task<Article> GetByIdAsync(int id);
		Task<Article> CreateAsync(Article article);

		Task UpdateAsync(Article article);
		Task UpdateAsync(Article existingEntity, Article model);
		void UpdateMany(IEnumerable<Article> articles);
		Task RemoveAsync(Article article);

		
	}

	public class ArticlesService : IArticlesService
	{
		private readonly IDefaultRepository<Article> _articleRepository;

		public ArticlesService(IDefaultRepository<Article> articleRepository)
		{
			_articleRepository = articleRepository;
		}

		
		public async Task<IEnumerable<Article>> FetchAsync()
			=> await _articleRepository.ListAsync(new ArticleFilterSpecification());
		

		public async Task<Article> GetByIdAsync(int id) => await _articleRepository.GetByIdAsync(id);
		
		public async Task<Article> CreateAsync(Article article) => await _articleRepository.AddAsync(article);

		

		

		public async Task UpdateAsync(Article article) => await _articleRepository.UpdateAsync(article);

		public async Task UpdateAsync(Article existingEntity, Article model) => await _articleRepository.UpdateAsync(existingEntity, model);

		public void UpdateMany(IEnumerable<Article> articles) => _articleRepository.UpdateRange(articles);


		public async Task RemoveAsync(Article article)
		{
			
			article.Removed = true;
			await _articleRepository.UpdateAsync(article);
		}

		
	}
}
