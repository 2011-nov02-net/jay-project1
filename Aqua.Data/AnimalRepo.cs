using Aqua.Data.Model;
using Aqua.Library;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Data
{
    public class AnimalRepo : IAnimalRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;

        public AnimalRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public List<Animal> GetAllAnimals()
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<AnimalEntity> dbAnimals = context.Animals.Distinct().ToList();
            List<Animal> result = new List<Animal>();
            foreach (AnimalEntity animal in dbAnimals)
            {
                Animal newAnimal = new Animal()
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Price = animal.Price
                };
                result.Add(newAnimal);
            };
            return result;
        }
        public Animal GetAnimalByName(string name)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            AnimalEntity dbAnimal = context.Animals
                .Where(a => a.Name == name)
                .FirstOrDefault();
            if (dbAnimal == null)
            {
                return null;
            }
            else
            {
                Animal newAnimal = new Animal()
                {
                    Id = dbAnimal.Id,
                    Name = dbAnimal.Name,
                    Price = dbAnimal.Price
                };
                return newAnimal;
            }
        }
        public Animal GetAnimalById(int id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            AnimalEntity dbAnimal = context.Animals
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (dbAnimal == null)
            {
                return null;
            }
            else
            {
                Animal newAnimal = new Animal()
                {
                    Id = dbAnimal.Id,
                    Name = dbAnimal.Name,
                    Price = dbAnimal.Price
                };
                return newAnimal;
            }
        }
        public void CreateAnimalEntity(Animal animal)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            AnimalEntity newEntry = new AnimalEntity()
            {
                Name = animal.Name,
                Price = animal.Price
            };
            context.Animals.Add(newEntry);
            context.SaveChanges();
        }
        public void UpdateAnimalEntity(Animal animal)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            AnimalEntity dbAnimal = context.Animals
                .Where(a => a.Id == animal.Id)
                .FirstOrDefault();
            dbAnimal.Name = animal.Name;
            dbAnimal.Price = animal.Price;
            context.SaveChanges();
        }
    }
}
