using ApplicationCore.DtoMapper;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DI
{
	public static class DtoMapperDI
	{
		public static void AddDtoMapper(this IServiceCollection services)
		{
			IMapper mapper = MappingConfig.CreateConfiguration().CreateMapper();
			services.AddSingleton(mapper);
		}
	}
}
