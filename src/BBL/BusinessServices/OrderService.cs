using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Order;
using Application.EntitiesModels.Models.OrderModels;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.BBL.BusinessServices
{
    public class OrderService : IOrderService
    {
        private readonly IDbContextFactory _dbContextFactory;

        private readonly IModelMapper modelMapper;
        private readonly ISmtpClient _smtpClient;

        public OrderService(IDbContextFactory dbContextFactory, IModelMapper modelMapper, ISmtpClient smtpClient)
        {
            _dbContextFactory = dbContextFactory;
            this.modelMapper = modelMapper;
            this._smtpClient = smtpClient;
        }

        private void SendCreationLetter(string orderNumber, int? statusId, int userId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var curruntUser = context.ApplicationUsers.AsNoTracking().FirstOrDefault(user => user.Id == userId);
                if (curruntUser == null)
                    throw new Exception("User not found.");
                var orderStatus = context.OrderStatuses.AsNoTracking().FirstOrDefault(status => status.Id == statusId);
                if (orderStatus == null)
                    throw new Exception("Status not found.");
                _smtpClient.SendMail(curruntUser.Email, $"Your order {orderNumber} has been created", $"Your order {orderNumber} has status: {orderStatus.StatusName}");
            }
        }
        public OrderModel AddOrUpdateOrder(OrderModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                if (model.Id == Guid.Empty)
                {
                    var newId = Guid.NewGuid();
                    var idStr = newId.ToString();

                    var createdOrderStatus = context.OrderStatuses.FirstOrDefault(status => status.StatusName == "Created");

                    // If you get this error, check the table of order statuses, most likely it is empty.
                    if (createdOrderStatus == null)
                        throw new Exception("No status was found in the database for the created order.");

                    var order = new Order
                    {
                        Id = newId,
                        UserId = model.User.Id,
                        CreatedDate = model.CreatedDate,
                        CountOfWares = model.CountOfWares,
                        OrderStatusId = createdOrderStatus.Id,
                        DeliveryServiceId = null,
                        DeclarationNumber = model.DeclarationNumber,
                        OrderNumber = idStr.ToString().Substring(idStr.LastIndexOf('-') + 1, idStr.Length - idStr.LastIndexOf('-') - 1)
                    };

                    var orderDetailList = new List<OrderDetails>();

                    foreach (var orderDetail in model.OrderDetails)
                    {
                        orderDetailList.Add(new OrderDetails
                        {
                            Order = order,
                            WareId = orderDetail.Ware.Id,
                            Count = orderDetail.Count
                        });
                    }

                    var orderHistory = new OrderHistory
                    {
                        Date = DateTime.Now,
                        Message = "Заявка создана",
                        Order = order,
                        UserId = model.User.Id
                    };

                    context.Orders.Add(order);
                    context.OrderDetails.AddRange(orderDetailList);
                    context.OrderHistories.Add(orderHistory);

                    context.SaveChanges();
                    this.SendCreationLetter(order.OrderNumber, order.OrderStatusId, order.UserId);

                    return modelMapper.MapTo<Order, OrderModel>(order);
                }
                else
                {
                    // TODO: Full update order
                    Order order = context.Orders.Where(_ => _.Id == model.Id).Include(_ => _.OrderStatus).FirstOrDefault();

                    if (order != null)
                    {
                        OrderStatus orderStatus = context.OrderStatuses.FirstOrDefault(_ => _.StatusName == model.Status);

                        if (orderStatus != null)
                        {
                            DeliveryService deliveryService = context.DeliveryServices.FirstOrDefault(_ => _.Name == model.DeliveryService);
                            order.OrderStatusId = orderStatus.Id;
                            order.DeliveryServiceId = deliveryService != null ? deliveryService.Id : order.DeliveryServiceId;
                            order.DeclarationNumber = model.DeclarationNumber;
                            context.SaveChanges();
                            return modelMapper.MapTo<Order, OrderModel>(order);
                        }
                        else
                        {
                            throw new Exception($"Order status with text {model.Status} not found");
                        }
                    }
                    else
                    {
                        throw new Exception($"Order with id {model.Id} not found");
                    }
                }

                return null;

            }
        }

        public bool Delete(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var order = context.Orders.FirstOrDefault(_ => _.Id == id);

                if (order == null)
                    throw new Exception("Order not found");

                context.Orders.Remove(order);

                context.SaveChanges();

                return true;
            }
        }

        public List<OrderModel> OrderQuery(QueryOrderModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var orderQuery = context.Orders.Include(x => x.OrderHistories).Include(x => x.OrderDetails).ThenInclude(x => x.Ware).Include(x => x.OrderStatus).AsQueryable();

                if (model.OrderNumberContains != null)
                {
                    orderQuery = orderQuery.Where(x => x.OrderNumber.Contains(model.OrderNumberContains));
                }
                if (model.UserId != null && model.UserId > 0)
                {
                    orderQuery = orderQuery.Where(x => x.UserId == model.UserId);
                }

                orderQuery.Skip(model.Skip ?? 0);

                orderQuery.Take(model.Take ?? 10);

                var result = orderQuery.ToList();

                return modelMapper.MapTo<List<Order>, List<OrderModel>>(result);
            }
        }

        public QueryOrderModel Get(QueryOrderModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryOrderModel();

                var query = context.Orders.Include(_ => _.OrderDetails).ThenInclude(_ => _.Ware).Include(_ => _.OrderHistories).Include(_ => _.User).Include(_ => _.OrderStatus).Include(_ => _.DeliveryService).AsQueryable();

                if (!String.IsNullOrEmpty(queryModel.UserNameContains))
                    query = query.Where(x => x.User.Name.Contains(queryModel.UserNameContains));

                if (!String.IsNullOrEmpty(queryModel.UserEmailContains))
                    query = query.Where(x => x.User.Email.ToString().Contains(queryModel.UserEmailContains));

                if (!String.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("orderNumber"))
                        query = query.OrderBy(x => x.OrderNumber);

                    else if (queryModel.OrderBy.Contains("name"))
                        query = query.OrderBy(x => x.User.Name);

                    else if (queryModel.OrderBy.Contains("email"))
                        query = query.OrderBy(x => x.User.Email);

                }

                if (!String.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderByDesc.Contains("orderNumber"))
                        query = query.OrderByDescending(x => x.OrderNumber);

                    else if (queryModel.OrderByDesc.Contains("name"))
                        query = query.OrderByDescending(x => x.User.Name);

                    else if (queryModel.OrderByDesc.Contains("email"))
                        query = query.OrderByDescending(x => x.User.Email);
                }

                resultModel.TotalCount = query.Count();

                query = query.Skip(queryModel.Skip ?? 0);

                query = query.Take(queryModel.Take ?? 10);

                resultModel.Result = modelMapper.MapTo<List<Order>, List<OrderModel>>(query.ToList());

                return resultModel;
            }
        }

        public IEnumerable<OrderStatusModel> GetStatuses()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.OrderStatuses.ToList().Select(_ => new OrderStatusModel() { Id = _.Id, StatusName = _.StatusName });
            }
        }

        public OrderModel GetById(Guid guid)
        {
            using (var context = _dbContextFactory.Create())
            {
                var order = context.Orders.Where(_ => _.Id == guid).Include(_ => _.User).Include(_ => _.OrderStatus).Include(_ => _.DeliveryService).FirstOrDefault();

                if (order == null)
                    throw new Exception($"Order with id {guid} not found");

                return modelMapper.MapTo<Order, OrderModel>(order);
            }
        }

        public IEnumerable<DeliveryServiceModel> GetDeliveryServices()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.DeliveryServices.ToList().Select(_ => new DeliveryServiceModel() { Id = _.Id, Name = _.Name });
            }
        }
    }
}
