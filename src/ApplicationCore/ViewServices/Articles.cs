using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Views;
using ApplicationCore.Models;
using ApplicationCore.Paging;
using ApplicationCore.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Views;
using AutoMapper;
using Newtonsoft.Json;
using System.Runtime.InteropServices.ComTypes;

namespace ApplicationCore.ViewServices
{
	public static class ArticlesViewService
	{
		public static ArticleViewModel MapViewModel(this Article article, IMapper mapper) 
			=> mapper.Map<ArticleViewModel>(article);

		public static List<ArticleViewModel> MapViewModelList(this IEnumerable<Article> articles, IMapper mapper) 
			=> articles.Select(item => MapViewModel(item, mapper)).ToList();

		public static PagedList<Article, ArticleViewModel> GetPagedList(this IEnumerable<Article> articles, IMapper mapper, int page = 1, int pageSize = 999)
		{
			var pageList = new PagedList<Article, ArticleViewModel>(articles, page, pageSize);

			pageList.ViewList = pageList.List.MapViewModelList(mapper);

			pageList.List = null;

			return pageList;
		}

		public static Article MapEntity(this ArticleViewModel model, IMapper mapper, string currentUserId)
		{ 
			var entity = mapper.Map<ArticleViewModel, Article>(model);

			if (model.Id == 0) entity.SetCreated(currentUserId);
			else entity.SetUpdated(currentUserId);

			return entity;
		}

		public static IEnumerable<Article> GetOrdered(this IEnumerable<Article> articles)
			=> articles.OrderByDescending(item => item.CreatedAt);



		
	}
}
