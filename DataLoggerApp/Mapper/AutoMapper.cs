using System.Collections.Generic;
using AutoMapper;
using DataLoggerApp.Models;

namespace DataLoggerApp.Mapper
{
    public class ObjectMapper
    {
        public static IMapper Mapper
        {
            get
            {
                return AutoMapper.Mapper.Instance;
            }
        }
        static ObjectMapper()
        {
            CreateMap();
        }

        private static void CreateMap()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;
                cfg.CreateMap<DataTrace, Data>().ReverseMap();
                cfg.CreateMap<List<DataTrace>, List<Data>>().ReverseMap();

            });
        }
    }
}
