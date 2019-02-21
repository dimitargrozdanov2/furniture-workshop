using FurnitureManufacturer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureManufacturer.Engine
{
    public class DataBase : IDataBase
    {
        private IDictionary<string, ICompany> companies;
        private IDictionary<string, IFurniture> furnitures;

        public DataBase()
        {
            this.Companies = new Dictionary<string, ICompany>();
            this.Furnitures = new Dictionary<string, IFurniture>();
        }

        public IDictionary<string, ICompany> Companies
        {
            get
            {
                return this.companies;
            }
            set
            {
                this.companies = value;
            }
        }
        public IDictionary<string, IFurniture> Furnitures
        {
            get
            {
                return this.furnitures;
            }

            set
            {
                this.furnitures = value;
            }
        }
    }
}
