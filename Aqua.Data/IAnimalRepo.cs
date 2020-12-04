using Aqua.Library;
using System.Collections.Generic;

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
