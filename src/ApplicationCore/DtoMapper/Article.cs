using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DtoMapper
{
    public class ArticleMappingProfile : AutoMapper.Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<Article, ArticleViewModel>();

            CreateMap<ArticleViewModel, Article>();
        }
    }
}
