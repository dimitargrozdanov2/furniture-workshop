﻿using FurnitureManufacturer.Engine.Factories;
using FurnitureManufacturer.Interfaces;
using FurnitureManufacturer.Interfaces.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FurnitureManufacturer.Engine
{
    public sealed class FurnitureManufacturerEngine : IFurnitureManufacturerEngine
    {

        private readonly ICompanyFactory companyFactory;
        private readonly IFurnitureFactory furnitureFactory;
        private readonly IDataBase database;
        private readonly IRenderer renderer;

        private FurnitureManufacturerEngine(ICompanyFactory companyFactory, IFurnitureFactory furnitureFactory, IDataBase database, IRenderer renderer)
        {
            this.companyFactory = companyFactory;
            this.furnitureFactory = furnitureFactory;
            this.database = database;
            this.renderer = renderer;
        }

      

        public void Start()
        {
            var commandResults = new List<string>();

            try
            {
                var commands = this.ReadCommands();
                commandResults = this.ProcessCommands(commands).ToList();
            }
            catch (Exception ex)
            {
                commandResults.Add(ex.Message);
            }

            this.RenderCommandResults(commandResults);

        }

        private ICollection<ICommand> ReadCommands()
        {
            var commands = new List<ICommand>();
            foreach (var currentLine in this.renderer.Input())
            {
                var currentCommand = Command.Parse(currentLine);
                commands.Add(currentCommand);
            }

            return commands;
        }

        private IEnumerable<string> ProcessCommands(ICollection<ICommand> commands)
        {
            var commandResults = new List<string>();

            foreach (var command in commands)
            {
                string commandResult;

                switch (command.Name)
                {
                    case EngineConstants.CreateCompanyCommand:
                        var companyName = command.Parameters[0];
                        var companyRegistrationNumber = command.Parameters[1];
                        commandResult = this.CreateCompany(companyName, companyRegistrationNumber);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.AddFurnitureToCompanyCommand:
                        var companyToAddTo = command.Parameters[0];
                        var furnitureToBeAdded = command.Parameters[1];
                        commandResult = this.AddFurnitureToCompany(companyToAddTo, furnitureToBeAdded);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.RemoveFurnitureFromCompanyCommand:
                        var companyToRemoveFrom = command.Parameters[0];
                        var furnitureToBeRemoved = command.Parameters[1];
                        commandResult = this.RemoveFurnitureFromCompany(companyToRemoveFrom, furnitureToBeRemoved);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.FindFurnitureFromCompanyCommand:
                        var companyToFindFrom = command.Parameters[0];
                        var furnitureToBeFound = command.Parameters[1];
                        commandResult = this.FindFurnitureFromCompany(companyToFindFrom, furnitureToBeFound);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.ShowCompanyCatalogCommand:
                        var companyToShowCatalog = command.Parameters[0];
                        commandResult = this.ShowCompanyCatalog(companyToShowCatalog);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.CreateTableCommand:
                        var tableModel = command.Parameters[0];
                        var tableMaterial = command.Parameters[1];
                        var tablePrice = decimal.Parse(command.Parameters[2]);
                        var tableHeight = decimal.Parse(command.Parameters[3]);
                        var tableLength = decimal.Parse(command.Parameters[4]);
                        var tableWidth = decimal.Parse(command.Parameters[5]);
                        commandResult = this.CreateTable(tableModel, tableMaterial, tablePrice, tableHeight, tableLength, tableWidth);
                        commandResults.Add(commandResult);
                        break;
                    case EngineConstants.CreateChairCommand:
                        var chairModel = command.Parameters[0];
                        var chairMaterial = command.Parameters[1];
                        var chairPrice = decimal.Parse(command.Parameters[2]);
                        var chairHeight = decimal.Parse(command.Parameters[3]);
                        var chairLegs = int.Parse(command.Parameters[4]);
                        commandResult = this.CreateChair(chairModel, chairMaterial, chairPrice, chairHeight, chairLegs);
                        commandResults.Add(commandResult);
                        break;
                    default:
                        commandResults.Add(string.Format(EngineConstants.InvalidCommandErrorMessage, command.Name));
                        break;
                }
            }

            return commandResults;
        }

        private void RenderCommandResults(IEnumerable<string> output)
        {
            this.renderer.Output(output);
        }

        private string CreateCompany(string name, string registrationNumber)
        {
            if (this.companies.ContainsKey(name))
            {
                return string.Format(EngineConstants.CompanyExistsErrorMessage, name);
            }

            var company = this.companyFactory.CreateCompany(name, registrationNumber);
            this.companies.Add(name, company);

            return string.Format(EngineConstants.CompanyCreatedSuccessMessage, name);
        }

        private string AddFurnitureToCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            if (!this.furnitures.ContainsKey(furnitureName))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            var company = this.companies[companyName];
            var furniture = this.furnitures[furnitureName];
            company.Add(furniture);

            return string.Format(EngineConstants.FurnitureAddedSuccessMessage, furnitureName, companyName);
        }

        private string RemoveFurnitureFromCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            if (!this.furnitures.ContainsKey(furnitureName))
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            var company = this.companies[companyName];
            var furniture = this.furnitures[furnitureName];
            company.Remove(furniture);

            return string.Format(EngineConstants.FurnitureRemovedSuccessMessage, furnitureName, companyName);
        }

        private string FindFurnitureFromCompany(string companyName, string furnitureName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            var company = this.companies[companyName];
            var furniture = company.Find(furnitureName);
            if (furniture == null)
            {
                return string.Format(EngineConstants.FurnitureNotFoundErrorMessage, furnitureName);
            }

            return furniture.ToString();
        }

        private string ShowCompanyCatalog(string companyName)
        {
            if (!this.companies.ContainsKey(companyName))
            {
                return string.Format(EngineConstants.CompanyNotFoundErrorMessage, companyName);
            }

            return this.companies[companyName].Catalog();
        }

        private string CreateTable(string model, string material, decimal price, decimal height, decimal length, decimal width)
        {
            if (this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureExistsErrorMessage, model);
            }

            var table = (IFurniture)this.furnitureFactory.CreateTable(model, material, price, height, length, width);
            this.furnitures.Add(model, table);

            return string.Format(EngineConstants.TableCreatedSuccessMessage, model);
        }

        private string CreateChair(string model, string material, decimal price, decimal height, int legs)
        {
            if (this.furnitures.ContainsKey(model))
            {
                return string.Format(EngineConstants.FurnitureExistsErrorMessage, model);
            }

            IFurniture chair = this.furnitureFactory.CreateChair(model, material, price, height, legs);
            this.furnitures.Add(model, chair);

            return string.Format(EngineConstants.ChairCreatedSuccessMessage, model);
        }
    }
}
