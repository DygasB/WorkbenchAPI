using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Authorization;
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
        private readonly IAuthorizationService _authorizationHandler;
        private readonly IClientContextService _clientContextService;

        public ShopService(WorkbenchDbContext workbenchDbContext, IMapper mapper, ILogger<ShopService> logger,
            IAuthorizationService authorizationHandler, IClientContextService clientContextService)
        {
            _workbenchDbContext = workbenchDbContext;
            _mapper = mapper;
            _logger = logger;   
            _authorizationHandler = authorizationHandler;
            _clientContextService = clientContextService;
        }

        public PagedResult<ShopDto>? GetAll(ShopQuery? query)
        {
            var baseQuery = _workbenchDbContext
                .Shops
                .Include(x => x.Products)
                .Where(r => query.SearchPhrase == null ||
                (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                || r.Description.ToLower().Contains(query.SearchPhrase.ToLower()))); // wykonujemy od lewej do prawej, jesli pierwsze wyrazenie da 0, dalej juz nie sprawdza

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Shop, object>>>
                {
                    {nameof(Shop.Name) , r=>r.Name},
                    {nameof(Shop.Description) , r=>r.Description},
                    {nameof(Shop.Category) , r=>r.Category}
                };
                var selectedColumn = columnsSelectors[query.SortBy];
                baseQuery= query.SortDirection ==SortDirection.ASC ?
                    baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var shops = 
                baseQuery
                .Skip(query.PageSize * (query.PageNumber-1))
                .Take(query.PageSize)
                .ToList();
            var totalItemsCount = baseQuery.Count();
            var result = _mapper.Map<List<ShopDto>>(shops);
            var resultPage = new PagedResult<ShopDto>(result, totalItemsCount, query.PageSize, query.PageNumber);
            return resultPage;
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

        public int CreateShop(CreateShopDto dto)
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
        public Shop Patch(int id, JsonPatchDocument<Shop> patchShop)
        {
            var shop = _workbenchDbContext.Shops.FirstOrDefault(x => x.Id == id);
            if (shop is null)
                throw new NotFoundExceptions("Shop not found");
            patchShop.ApplyTo(shop);
            _workbenchDbContext.SaveChanges();
            return shop;
            
        }
        
    }
}
