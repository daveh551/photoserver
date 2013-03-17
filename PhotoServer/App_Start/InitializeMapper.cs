using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace PhotoServer.App_Start
{
	public static class InitializeMapper
	{
		public static void MapClasses()
		{
			Mapper.CreateMap<PhotoServer.Domain.Photo, PhotoServer.Models.PhotoData>();
			Mapper.AssertConfigurationIsValid();
		}
	}
}