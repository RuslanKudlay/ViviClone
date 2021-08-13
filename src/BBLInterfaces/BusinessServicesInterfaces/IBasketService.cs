using Application.EntitiesModels.Models.ViewsModel;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IBasketService
    {
        BasketModel GetBasket();
        void AddWareToBasket(int id);
        void AddWareToBasket(string subUrl);
        int GetWareQuantityInBasket();
        void RemoveItemFromBasket(int id);
        void SetCountWareInBasket(int id, int wareCount);
        void ClearBasket();
    }
}
