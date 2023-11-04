using AutoMapper;
using GeekShopping.Coupon.Data.DTO;

namespace GeekShopping.CouponAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<CouponDTO, GeekShopping.CouponAPI.Model.Coupon>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
