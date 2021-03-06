﻿namespace Eugenie.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(string name);

        int Count();

        void AddOrUpdate(string name, string newName, decimal? buyingPrice, decimal? sellingPrice, MeasureType? measure, decimal? quantity, ICollection<Barcode> barcodes, ICollection<ExpirationDate> expirationDates);

        IQueryable<Product> All();

        Product GetByBarcode(string barcode);

        IQueryable<Product> GetByQuantity(decimal quantity);

        IQueryable<Product> GetByName(string name);

        IQueryable<Product> GetById(int id);

        IQueryable<Product> GetByExpirationDate(int days);
    }
}