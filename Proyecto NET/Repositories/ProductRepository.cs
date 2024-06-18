﻿using ExtensionMethods;
using Proyecto_NET.Data;
using Proyecto_NET.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
namespace Proyecto_NET.Repositories
{
    public class ProductRepository : iProductRepository
    {
        private readonly AdventureWorksLT2022 _dbContext;
        public ProductRepository() {
            _dbContext = new AdventureWorksLT2022();
        }
        public List<Product> getProducts()
        {
      
            var query = from p in _dbContext.Products
                        // Logic delete clause
                        where p.DiscontinuedDate == null
                        orderby p.ProductID
                        select p;

            return query.ToList();

        }

        public List<Product> getFilteredProducts(Expression<Func<Product, bool>> filter, string orderBy)
        {
            var products = _dbContext.Products;
            var query = products.Where(filter).OrderBy(orderBy);

            return query.ToList();
        }

        

        public bool deleteProduct(int id)
        {
            try
            {
                var productToRemove = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);

                if (productToRemove != null)
                {
                    productToRemove.DiscontinuedDate = DateTime.Now;
                    _dbContext.Products.AddOrUpdate(productToRemove);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto");
            }
            
            
        }

        public Product addProduct (Product product)
        {
            product.ModifiedDate = DateTime.Now;
            product.rowguid = Guid.NewGuid();
            Product productAdded = _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return productAdded;
        }

        public Product updateProduct(Product product) {
            product.ModifiedDate = DateTime.Now;
            _dbContext.Products.AddOrUpdate(product);
            _dbContext.SaveChanges();
            return product;
        }

        public Product updateProductFields(int id, Product product) {
            Product productToUpdate = _dbContext.Products.SingleOrDefault(p => p.ProductID == id);

            productToUpdate.Name = product.Name;
            productToUpdate.Color = product.Color;
            productToUpdate.Size = product.Size;
            productToUpdate.ListPrice = product.ListPrice;
            productToUpdate.Weight = product.Weight;
            product.ModifiedDate = DateTime.Now;

            _dbContext.Products.AddOrUpdate(productToUpdate);
            _dbContext.SaveChanges();

            return product;
        }


    }
}