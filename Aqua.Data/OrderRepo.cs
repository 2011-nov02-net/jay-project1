using Aqua.Data.Model;
using Aqua.Library;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Data
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;
        private readonly CustomerRepo _customerRepo;
        private readonly LocationRepo _locationRepo;
        public OrderRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
            _locationRepo = new LocationRepo(contextOptions);
            _customerRepo = new CustomerRepo(contextOptions);
        }
        public List<Order> GetAllOrders()
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<OrderEntity> dbOrders = context.Orders
                .Include(o => o.Location)
                .Include(o => o.Customer)
                .ToList();
            List<Order> result = new List<Order>();
            foreach (OrderEntity order in dbOrders)
            {
                Location newLocation = _locationRepo.GetLocationById(order.LocationId);
                Customer newCust = _customerRepo.GetCustomerById(order.CustomerId);
                Order newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust,
                    Total = order.Total,
                    Date = order.Date
                };
                result.Add(newOrder);
            };
            return result;
        }
        public Order GetOrderById(int id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderEntity dbOrder = context.Orders
                .Where(l => l.Id == id)
                .FirstOrDefault();
            if (dbOrder == null)
            {
                return null;
            }
            Order result = new Order()
            {
                Id = dbOrder.Id,
                Location = _locationRepo.GetLocationById(dbOrder.LocationId),
                Customer = _customerRepo.GetCustomerById(dbOrder.CustomerId),
                Total = dbOrder.Total,
                Date = dbOrder.Date
            };
            List<OrderItem> orderItems = GetOrderItemsByOrder(result);
            foreach (OrderItem thing in orderItems)
            {
                result.OrderItems.Add(thing);
            }
            return result;
        }
        public List<Order> GetOrdersByLocation(Location location)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerRepo custRepo = new CustomerRepo(_contextOptions);
            List<OrderEntity> dbOrders = context.Orders
                .Where(o => o.LocationId == location.Id)
                .Include(o => o.Location)
                .Include(o => o.Customer)
                .ToList();
            List<Order> result = new List<Order>();
            foreach (OrderEntity order in dbOrders)
            {
                Location newLocation = _locationRepo.GetLocationById(order.LocationId);
                Customer newCust = _customerRepo.GetCustomerById(order.CustomerId);
                Order newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust,
                    Total = order.Total
                };
                newOrder.Id = order.Id;
                newOrder.Date = order.Date;
                List<OrderItem> newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach (OrderItem orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }
        public List<Order> GetOrdersByCustomer(Customer customer)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<OrderEntity> dbOrders = context.Orders
                .Where(o => o.CustomerId == customer.Id)
                .ToList();
            List<Order> result = new List<Order>();
            foreach (OrderEntity order in dbOrders)
            {
                Location newLocation = _locationRepo.GetLocationById(order.LocationId);
                Customer newCust = _customerRepo.GetCustomerById(order.CustomerId);
                Order newOrder = new Order()
                {
                    Id = order.Id,
                    Location = newLocation,
                    Customer = newCust,
                    Total = order.Total
                };
                newOrder.Id = order.Id;
                newOrder.Date = order.Date;
                List<OrderItem> newOrderItems = GetOrderItemsByOrder(newOrder);
                foreach (OrderItem orderItem in newOrderItems)
                {
                    newOrder.OrderItems.Add(orderItem);
                }
                result.Add(newOrder);
            }
            return result;
        }
        public List<OrderItem> GetOrderItemsByOrder(Order order)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<OrderItemEntity> dbOrderItems = context.OrderItems
                .Where(o => o.OrderId == order.Id)
                .Include(o => o.Animal)
                .ToList();
            List<OrderItem> result = new List<OrderItem>();
            foreach (OrderItemEntity orderItem in dbOrderItems)
            {
                Animal newAnimal = new Animal()
                {
                    Id = orderItem.Animal.Id,
                    Name = orderItem.Animal.Name,
                    Price = orderItem.Animal.Price
                };
                OrderItem newOrderItem = new OrderItem(orderItem.OrderId, newAnimal, orderItem.Quantity, orderItem.Total)
                {
                    Id = orderItem.Id
                };
                result.Add(newOrderItem);
            }
            return result;
        }
        public OrderItem GetOrderItemById(int id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderItemEntity dbOrderItem = context.OrderItems
                .Where(o => o.Id == id)
                .Include(o => o.Animal)
                .FirstOrDefault();
            if (dbOrderItem == null)
            {
                return null;
            }
            Animal newAnimal = new Animal()
            {
                Id = dbOrderItem.Animal.Id,
                Name = dbOrderItem.Animal.Name,
                Price = dbOrderItem.Animal.Price
            };
            OrderItem result = new OrderItem(dbOrderItem.OrderId, newAnimal, dbOrderItem.Quantity, dbOrderItem.Total)
            {
                Id = dbOrderItem.Id
            };
            return result;
        }
        public void CreateOrderEntity(Order order)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderEntity orderEntry = new OrderEntity()
            {
                CustomerId = order.Customer.Id,
                LocationId = order.Location.Id,
                Date = order.Date,
                Total = order.Total
            };
            context.Orders.Add(orderEntry);
            context.SaveChanges();
            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderEntry.Id;
                CreateOrderItemEntity(orderItem);
            }
            context.SaveChanges();
        }
        public Order CreateOrderEntityReturnIt(Order order)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderEntity orderEntry = new OrderEntity()
            {
                CustomerId = order.Customer.Id,
                LocationId = order.Location.Id,
                Date = order.Date,
                Total = order.Total
            };
            context.Orders.Add(orderEntry);
            context.SaveChanges();
            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderEntry.Id;
                CreateOrderItemEntity(orderItem);
            }
            context.SaveChanges();
            return GetOrderById(orderEntry.Id);
        }

        public void CreateOrderItemEntity(OrderItem orderItem)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderItemEntity orderItemEntry = new OrderItemEntity()
            {
                OrderId = orderItem.OrderId,
                AnimalId = orderItem.Animal.Id,
                Quantity = orderItem.Quantity,
                Total = orderItem.Total
            };
            context.OrderItems.Add(orderItemEntry);
            context.SaveChanges();
        }
        public void UpdateOrderEntity(Order order)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderEntity dbOrder = context.Orders
                .Where(o => o.Id == order.Id)
                .FirstOrDefault();
            dbOrder.Total = order.Total;
            context.SaveChanges();
        }
        public void UpdateOrderItemEntity(OrderItem orderItem)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderItemEntity dbOrderItem = context.OrderItems
                .Where(o => o.Id == orderItem.Id)
                .FirstOrDefault();
            dbOrderItem.Quantity = orderItem.Quantity;
            dbOrderItem.Total = orderItem.Total;
            context.SaveChanges();
        }
        public void DeleteOrderEntity(Order order)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            OrderEntity dbOrder = context.Orders
                .Where(i => i.Id == order.Id)
                .FirstOrDefault();
            context.Remove(dbOrder);
            context.SaveChanges();
        }
    }
}