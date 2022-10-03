using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public interface IShopService
    {
        int CreateRestaurant(CreateShopDto dto);
        List<ShopDto>? GetAll();
        ShopDto? GetById(int id);
        void DeleteById(int id);
        void UpdatePut(int id, UpdateShopDto dto);
        

    }
}