using AutoMapper;

namespace SignalrDemo.Helper
{
    public static class CommonHelper
    {
        public static T1 ToDocumentData<T, T1>(this T model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, T1>();
            });
            IMapper iMapper = config.CreateMapper();
            T1 doc = iMapper.Map<T, T1>(model);
            return doc;
        }
    }
}
