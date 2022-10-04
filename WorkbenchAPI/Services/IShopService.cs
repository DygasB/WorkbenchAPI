using Microsoft.AspNetCore.JsonPatch;
using System.Security.Claims;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public interface IShopService
    {
        int CreateShop(CreateShopDto dto);
        PagedResult<ShopDto>? GetAll(ShopQuery query );
        ShopDto? GetById(int id);
        void DeleteById(int id);
        void UpdatePut(int id, UpdateShopDto dto);
        Shop Patch(int id, JsonPatchDocument<Shop> patchShop);



    }
}