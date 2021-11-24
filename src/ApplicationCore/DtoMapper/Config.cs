using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.DtoMapper
{
	public class MappingConfig
	{
		public static MapperConfiguration CreateConfiguration()
		{
			return new MapperConfiguration(cfg => {
				cfg.AddMaps(typeof(ArticleMappingProfile).Assembly);
			});
		}

	}

	
}
