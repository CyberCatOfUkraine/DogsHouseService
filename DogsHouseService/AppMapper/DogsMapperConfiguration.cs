using AutoMapper;

namespace DogsHouseService.AppMapper
{
    public static class DogsMapperConfiguration
    {
        public static MapperConfiguration GetConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DB.Model.Dog, Model.Dog>().ReverseMap();
            });
        }
    }
}
