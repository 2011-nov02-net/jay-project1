using System;
using System.Collections.Generic;
using System.Text;
using Aqua.Library;

namespace Aqua.Data
{
    public interface IAnimalRepo
    {
        List<Animal> GetAllAnimals();
        Animal GetAnimalByName(string name);
        Animal GetAnimalById(int id);
        void CreateAnimalEntity(Animal animal);
        void UpdateAnimalEntity(Animal animal);
    }
}
