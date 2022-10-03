using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Exceptions;
using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly WorkbenchDbContext _workbenchDbContext;
        private readonly IMapper _mapper;
        public ProductService(WorkbenchDbContext workbenchDbContext, IMapper mapper)
        {
            _workbenchDbContext = workbenchDbContext;
            _mapper = mapper;
        }
        public int Create(int shopId, CreateProductDto dto)
        {
            var shop = GetShopById(shopId);
            var productEntity = _mapper.Map<Product>(dto);
            productEntity.ShopId = shopId; // bez tego nie zadziala
            _workbenchDbContext.Products.Add(productEntity);
            _workbenchDbContext.SaveChanges();
            return productEntity.Id;
        }

        public ProductDto GetById(int shopId, int productId)
        {
            var shop = GetShopById(shopId);
            var product = _workbenchDbContext.Products.FirstOrDefault(x => x.Id == productId);
            if(product is null || product.ShopId != shopId)
                throw new NotFoundExceptions("Product not found");
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
        public List<ProductDto> GetAll(int shopId)
        {
            var shop = GetShopById(shopId);
            var productDtos = _mapper.Map<List<ProductDto>>(shop.Products);
            return productDtos;

        }

        public void Delete(int shopId)
        {
            var shop = GetShopById(shopId);
            var productsToDelete = shop.Products.ToList(); ;
            _workbenchDbContext.Products.RemoveRange(productsToDelete);
            _workbenchDbContext.SaveChanges();
        }
        public void DeleteById(int shopId, int productId)
        {
            var shop = GetShopById(shopId);
            
            var products = shop.Products.FirstOrDefault(x=>x.Id == productId);
            _workbenchDbContext.Products.Remove(products);
            _workbenchDbContext.SaveChanges();
        }

        private Shop GetShopById(int shopId)
        {
            var shop = _workbenchDbContext
                .Shops
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == shopId);
            if (shop is null)
                throw new NotFoundExceptions("Shop not found");
            return shop;
        }

    }
}
