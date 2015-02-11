﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Entities;

namespace NWheels.Puzzle.EntityFramework.ComponentTests
{
    public static class Interfaces
    {
        public static class Repository1
        {
            public interface IDataRepository : IApplicationDataRepository
            {
                IEntityRepository<IProduct> Products { get; }
                IEntityRepository<IOrder> Orders { get; }
            }
            [EntityContract]
            public interface IProduct
            {
                int Id { get; set; }
                string Name { get; set; }
                decimal Price { get; set; }
            }
            [EntityContract]
            public interface IOrder
            {
                int Id { get; set; }
                DateTime PlacedAt { get; set; }
                ICollection<IOrderLine> OrderLines { get; }
            }
            [EntityContract]
            public interface IOrderLine
            {
                int Id { get; set; }
                IOrder Order { get; set; }
                IProduct Product { get; set; }
                int Quantity { get; set; }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static class Repository2
        {
        }

    }
}
