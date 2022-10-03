using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Models;

namespace WorkbenchAPI
{
    public class ShopMappingProfile : Profile 
    {
        public ShopMappingProfile()
        {
            CreateMap<Shop, ShopDto>()
                .ForMember(so => so.City, x => x.MapFrom(s => s.Address.City))
                .ForMember(so => so.Street, x => x.MapFrom(s => s.Address.Street))
                .ForMember(so => so.PostalCode, x => x.MapFrom(s => s.Address.PostalCode));

            CreateMap<Product, ProductDto>();

            CreateMap<CreateShopDto, Shop>()
                .ForMember(s => s.Address,
                    x => x.MapFrom(csd => new Address() 
                        { City = csd.City, PostalCode = csd.PostalCode, Street = csd.Street }));
            
            CreateMap<UpdateShopDto, Shop>();
            CreateMap<CreateProductDto, Product>();
        }
    }
}
