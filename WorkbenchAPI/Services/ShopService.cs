using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Exceptions;
using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public class ShopService : IShopService
    {
        private readonly WorkbenchDbContext _workbenchDbContext;
        private readonly ILogger<ShopService> _logger;   
        private readonly IMapper _mapper;
        public ShopService(WorkbenchDbContext workbenchDbContext, IMapper mapper, ILogger<ShopService> logger)
        {
            _workbenchDbContext = workbenchDbContext;
            _mapper = mapper;
            _logger = logger;   
            
        }

        public List<ShopDto>? GetAll()
        {
            var shops = _workbenchDbContext.Shops.Include(x => x.Products).ToList();
            var result = _mapper.Map<List<ShopDto>>(shops);
            return result;
        }
        public ShopDto? GetById(int id)
        {
            var shop = _workbenchDbContext.Shops.Include(x => x.Products).FirstOrDefault(x => x.Id == id);
            if (shop is not null)
            {
                var result = _mapper.Map<ShopDto>(shop);
                return result;
            }
            throw new NotFoundExceptions("Shop not found");
        }

        public int CreateRestaurant(CreateShopDto dto)
        {
            var shop = _mapper.Map<Shop>(dto);
            _workbenchDbContext.Shops.Add(shop);
            _workbenchDbContext.SaveChanges();
            return shop.Id;
        }
        public void DeleteById(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");
            var shop = _workbenchDbContext.Shops.FirstOrDefault(x => x.Id == id);
            if (shop is null)
                throw new NotFoundExceptions("Shop not found");
            else
            {
                _workbenchDbContext.Remove(shop);
                _workbenchDbContext.SaveChanges();
                
            }

        }
        public void UpdatePut(int id, UpdateShopDto dto)
        {
            var shop = _workbenchDbContext.Shops.FirstOrDefault(x => x.Id == id);
            if (shop is null)
                throw new NotFoundExceptions("Shop not found");
            else
            {
                shop.Name = dto.Name;
                shop.Description = dto.Description;
                shop.HasDelivery = dto.HasDelivery;
                _workbenchDbContext.SaveChanges();
                
            }
        }


    }
}
