using Microsoft.AspNetCore.Mvc;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public interface IProductService
    {
        int Create([FromRoute] int shopId, [FromBody] CreateProductDto dto);
        ProductDto GetById(int shopId, int productId);
        List<ProductDto> GetAll(int shopId);
        void Delete(int shopId);
        void DeleteById(int shopId,int productId);
    }
}