using System.Collections.Generic;
using FurnitureManufacturer.Interfaces;

namespace FurnitureManufacturer.Engine
{
    public interface IDataBase
    {
        IDictionary<string, ICompany> Companies { get; set; }
        IDictionary<string, IFurniture> Furnitures { get; set; }
    }
}