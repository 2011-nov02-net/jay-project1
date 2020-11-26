using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aqua.Data.Model;
using Aqua.Library;

namespace Aqua.Data
{
    public class OrderRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;
        private CustomerRepo _customerRepo;
        private LocationRepo _locationRepo;
        public OrderRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
            _locationRepo = new LocationRepo(contextOptions);
            _customerRepo = new CustomerRepo(contextOptions);
        }
        public List<Order> GetAllOrders()
        {
            using var context = new AquaContext(_contextOptions);
            var dbOrders = context.Orders
                .Include(o => o.Location)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _locationRepo.GetLocationById(order.LocationId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust,
                    Total = order.Total
                };
                result.Add(newOrder);
            };
            return result;
        }
        public Order GetOrderById(int id){
            using var context = new AquaContext(_contextOptions);
            var dbOrder = context.Orders
                .Where(l => l.Id == id)
                .FirstOrDefault();
            var result = new Order()
            {
                Id = dbOrder.Id,
                Location = _locationRepo.GetLocationById(dbOrder.LocationId),
                Customer = _customerRepo.GetCustomerById(dbOrder.CustomerId),
                Total = dbOrder.Total
            };
            var orderItems = GetOrderItemsByOrder(result);
            foreach (var thing in orderItems)
            {
                result.OrderItems.Add(thing);
            }
            return result;
        }
        public List<Order> GetOrdersByLocation(Location location)
        {
            using var context = new AquaContext(_contextOptions);
            var custRepo = new CustomerRepo(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.LocationId == location.Id)
                .Include(o => o.Location)
                .Include(o => o.Customer)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _locationRepo.GetLocationById(order.LocationId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust
                };
                newOrder.Id = order.Id;
                newOrder.Date = order.Date;
                var newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach(var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }
        public List<Order> GetOrdersByCustomer(Customer customer)
        {
            using var context = new AquaContext(_contextOptions);
            var dbOrders = context.Orders
                .Where(o => o.CustomerId == customer.Id)
                .ToList();
            var result = new List<Order>();
            foreach (var order in dbOrders)
            {
                var newLocation = _locationRepo.GetLocationById(order.LocationId);
                var newCust = _customerRepo.GetCustomerById(order.CustomerId);
                var newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust
                };
                newOrder.Id = order.Id;
                newOrder.Date = order.Date;
                var newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach (var orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }
        public List<OrderItem> GetOrderItemsByOrder(Order order)
        {
            using var context = new AquaContext(_contextOptions);
            var dbOrderItems = context.OrderItems
                .Where(o => o.OrderId == order.Id)
                .ToList();
            var result = new List<OrderItem>();
            foreach(var orderItem in dbOrderItems)
            {
                var newOrderItem = new OrderItem(orderItem.OrderId, orderItem.AnimalId, orderItem.Quantity, orderItem.Total);
                result.Add(newOrderItem);
            }
            return result;
        }
        public void CreateOrderEntity(Order order)
        {
            using var context = new AquaContext(_contextOptions);
            var orderEntry = new OrderEntity()
            {
                CustomerId = order.Customer.Id,
                LocationId = order.Location.Id,
                Date = DateTime.Now,
                Total = order.Total
            };
            context.Orders.Add(orderEntry);
            context.SaveChanges();
            foreach(var orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderEntry.Id;
                CreateOrderItemEntity(orderItem);
            }
            context.SaveChanges();

        }
        public void CreateOrderItemEntity(OrderItem orderItem)
        {
            using var context = new AquaContext(_contextOptions);
            var orderItemEntry = new OrderItemEntity()
            {
                OrderId = orderItem.OrderId,
                AnimalId = orderItem.AnimalId,
                Quantity = orderItem.Quantity,
                Total = orderItem.Total
            };
            context.OrderItems.Add(orderItemEntry);
            context.SaveChanges();
        }
        public void UpdateOrderItemEntity(OrderItem orderItem)
        {
            using var context = new AquaContext(_contextOptions);
            var dbOrderItem = context.OrderItems
                .Where(o => o.Id == orderItem.Id)
                .FirstOrDefault();
            dbOrderItem.OrderId = orderItem.OrderId;
            dbOrderItem.AnimalId = orderItem.AnimalId;
            dbOrderItem.Quantity = orderItem.Quantity;
            dbOrderItem.Total = orderItem.Total;
            context.SaveChanges();
        }
    }
}
