using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Aqua.Data.Model;
using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;

namespace Aqua.WebApp.Utilities
{
    public class ValidOrderQuantity : ValidationAttribute
    {
        //protected override ValidationResult IsValid(int orderId, int animalId, int quantity, ICustomerRepo customerRepo, ILocationRepo locationRepo, IOrderRepo orderRepo, IAnimalRepo animalRepo)
        //{
        //    var currentOrder = orderRepo.GetOrderById(orderId);
        //    var currentLocationInventory = locationRepo.GetInvByLocation(currentOrder.Location);
        //    foreach (var inventoryAnimal in currentLocationInventory)
        //    {
        //        if (inventoryAnimal.Id == animalId)
        //        {
        //            if (inventoryAnimal.Quantity < quantity)
        //            {
        //                return new ValidationResult("TOO MUCH BRO");
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
