using Application.EntitiesModels.Models.OrderModels;
using Application.EntitiesModels.Models.QueryModels;
using System;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IOrderService
    {
        List<OrderModel> OrderQuery (QueryOrderModel model);
        OrderModel AddOrUpdateOrder(OrderModel model);
        QueryOrderModel Get(QueryOrderModel queryModel);
        OrderModel GetById(Guid guid);
        IEnumerable<OrderStatusModel> GetStatuses();
        IEnumerable<DeliveryServiceModel> GetDeliveryServices();
        bool Delete(Guid id);
    }
}
