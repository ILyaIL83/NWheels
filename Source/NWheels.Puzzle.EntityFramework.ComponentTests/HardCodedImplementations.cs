﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NWheels.Entities;
using NWheels.Puzzle.EntityFramework.Conventions;
using NWheels.Puzzle.EntityFramework.Impl;

namespace NWheels.Puzzle.EntityFramework.ComponentTests
{
    public static class HardCodedImplementations
    {
        public static class Repository1
        {
            public class DataRepositoryObject_DataRepository : ApplicationDataRepositoryBase, Interfaces.Repository1.IDataRepository
            {
                private IEntityRepository<Interfaces.Repository1.IOrder> m_Orders;
                private IEntityRepository<Interfaces.Repository1.IProduct> m_Products;

                public DataRepositoryObject_DataRepository(DbConnection connection, bool autoCommit)
                    : base(_s_compiledModel, connection, autoCommit)
                {
                    this.m_Products = new EntityRepository<Interfaces.Repository1.IProduct, EntityObject_Product>(this);
                    this.m_Orders = new EntityRepository<Interfaces.Repository1.IOrder, EntityObject_Order>(this);
                }

                public override sealed Type[] GetEntityTypesInRepository()
                {
                    return new Type[] { typeof(EntityObject_Product), typeof(EntityObject_Order) };
                }

                public IEntityRepository<Interfaces.Repository1.IOrder> Orders
                {
                    get
                    {
                        return this.m_Orders;
                    }
                }

                public IEntityRepository<Interfaces.Repository1.IProduct> Products
                {
                    get
                    {
                        return this.m_Products;
                    }
                }

                private static readonly DbCompiledModel _s_compiledModel;

                static DataRepositoryObject_DataRepository()
                {
                    var modelBuilder = new DbModelBuilder();

                    modelBuilder.Entity<EntityObject_Product>().HasEntitySetName("Product");
                    modelBuilder.Entity<EntityObject_Order>().HasEntitySetName("Order");
                    modelBuilder.Entity<EntityObject_OrderLine>().HasEntitySetName("OrderLine");

                    var model = modelBuilder.Build(new SqlConnection());
                    _s_compiledModel = model.Compile();
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------------------------------

            public class EntityObject_Order : Interfaces.Repository1.IOrder
            {
                private int m_Id;
                private ICollection<EntityObject_OrderLine> m_OrderLines = new HashSet<EntityObject_OrderLine>();
                private EntityObjectFactory.CollectionAdapter<EntityObject_OrderLine, Interfaces.Repository1.IOrderLine> m_OrderLines_Adapter;
                private DateTime m_PlacedAt;

                public EntityObject_Order()
                {
                    this.m_OrderLines_Adapter =
                        new EntityObjectFactory.CollectionAdapter<EntityObject_OrderLine, Interfaces.Repository1.IOrderLine>(this.m_OrderLines);
                }

                public int Id
                {
                    get { return this.m_Id; }
                    set { this.m_Id = value; }
                }

                ICollection<Interfaces.Repository1.IOrderLine> Interfaces.Repository1.IOrder.OrderLines
                {
                    get { return this.m_OrderLines_Adapter; }
                }

                public virtual ICollection<EntityObject_OrderLine> OrderLines
                {
                    get { return this.m_OrderLines; }
                    set
                    {
                        this.m_OrderLines_Adapter = new EntityObjectFactory.CollectionAdapter<EntityObject_OrderLine, Interfaces.Repository1.IOrderLine>(value);
                        this.m_OrderLines = value;
                    }
                }

                public DateTime PlacedAt
                {
                    get { return this.m_PlacedAt; }
                    set { this.m_PlacedAt = value; }
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------------------------------

            public class EntityObject_OrderLine : Interfaces.Repository1.IOrderLine
            {
                private int m_Id;
                private EntityObject_Order m_Order;
                private EntityObject_Product m_Product;
                private int m_Quantity;

                public int Id
                {
                    get { return this.m_Id; }
                    set { this.m_Id = value; }
                }

                Interfaces.Repository1.IOrder Interfaces.Repository1.IOrderLine.Order
                {
                    get { return this.m_Order; }
                    set { this.m_Order = (EntityObject_Order)value; }
                }

                Interfaces.Repository1.IProduct Interfaces.Repository1.IOrderLine.Product
                {
                    get { return this.m_Product; }
                    set
                    {
                        this.m_Product = (EntityObject_Product)value;
                    }
                }

                public virtual EntityObject_Order Order
                {
                    get { return this.m_Order; }
                    set { this.m_Order = value; }
                }

                public virtual EntityObject_Product Product
                {
                    get { return this.m_Product; }
                    set { this.m_Product = value; }
                }

                public int Quantity
                {
                    get { return this.m_Quantity; }
                    set { this.m_Quantity = value; }
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------------------------------

            public class EntityObject_Product : Interfaces.Repository1.IProduct
            {
                private int m_Id;
                private string m_Name;
                private decimal m_Price;

                public int Id
                {
                    get { return this.m_Id; }
                    set { this.m_Id = value; }
                }

                public string Name
                {
                    get { return this.m_Name; }
                    set { this.m_Name = value; }
                }

                public decimal Price
                {
                    get { return this.m_Price; }
                    set { this.m_Price = value; }
                }
            }
        }
    }
}
