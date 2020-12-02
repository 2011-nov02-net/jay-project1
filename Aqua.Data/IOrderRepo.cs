using System;
using System.Collections.Generic;
using System.Text;
using Aqua.Library;

namespace Aqua.Data
{
    public interface IOrderRepo
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        List<Order> GetOrdersByLocation(Location location);
        List<Order> GetOrdersByCustomer(Customer customer);
        List<OrderItem> GetOrderItemsByOrder(Order order);
        OrderItem GetOrderItemById(int id);
        void CreateOrderEntity(Order order);
        Order CreateOrderEntityReturnIt(Order order);
        void CreateOrderItemEntity(OrderItem orderItem);
        void UpdateOrderEntity(Order order);
        void UpdateOrderItemEntity(OrderItem orderItem);
        void DeleteOrderEntity(Order order);
    }
}
