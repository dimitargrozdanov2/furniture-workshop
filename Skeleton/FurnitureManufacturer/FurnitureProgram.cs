using Autofac;
using FurnitureManufacturer.Engine;
using FurnitureManufacturer.Interfaces.Engine;
using System.Reflection;

namespace FurnitureManufacturer
{
    public class FurnitureProgram
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            //Add singletons(single instances)

            // Add commands

            var container = builder.Build();

            var engine = container.Resolve<IFurnitureManufacturerEngine>();
            engine.Start(); 
            FurnitureManufacturerEngine.Instance.Start();
        }
    }
}
