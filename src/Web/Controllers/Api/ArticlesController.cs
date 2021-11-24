using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Helpers;
using AutoMapper;
using ApplicationCore.ViewServices;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers.Api
{
	[Authorize]
	public class ArticlesController : BaseApiController
	{
		private readonly IArticlesService _articlesService;
		private readonly IUsersService _usersService;
		private readonly IMapper _mapper;

		public ArticlesController(IArticlesService articlesService, IUsersService usersService, IMapper mapper)
		{
			_articlesService = articlesService;
			_usersService = usersService;
			_mapper = mapper;
		}
	

		[HttpGet("")]
		public async Task<ActionResult> Index(int page = 1, int pageSize = 99)
		{
			if (page < 1) page = 1;

			var articles = await _articlesService.FetchAsync();

			articles = articles.Where(x => x.Active);

			articles = articles.GetOrdered().ToList();

			return Ok(articles.GetPagedList(_mapper, page, pageSize));
		}


		[HttpGet("{id}/{user?}")]
		public async Task<ActionResult> Details(int id, string user = "")
		{
			var article = await _articlesService.GetByIdAsync(id);
			if (article == null) return NotFound();

			if (!article.Active)
			{
				var existingUser = await _usersService.FindUserByIdAsync(user);
				if(existingUser == null) return NotFound();

				bool isAdmin = await _usersService.IsAdminAsync(existingUser);
				if(!isAdmin) return NotFound();
			}

			return Ok(article.MapViewModel(_mapper));
		}



	}

	
}
