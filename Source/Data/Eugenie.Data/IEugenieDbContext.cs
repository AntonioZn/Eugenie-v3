﻿namespace Eugenie.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Models;

    public interface IEugenieDbContext
    {
        IDbSet<Barcode> Barcodes { get; set; }

        IDbSet<Report> Reports { get; set; }

        IDbSet<ExpirationDate> ExpirationDates { get; set; }

        IDbSet<MissingProduct> MissingProducts { get; set; }

        IDbSet<Product> Products { get; set; }

        IDbSet<Sell> Sells { get; set; }

        IDbSet<Waste> Waste { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void Dispose();

        int SaveChanges();
    }
}