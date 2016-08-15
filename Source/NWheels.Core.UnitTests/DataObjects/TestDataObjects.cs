﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Core.DataObjects;
using NWheels.DataObjects;

namespace NWheels.Core.UnitTests.DataObjects
{
    public static class TestDataObjects
    {
        [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
        public class TestDataContractAttribute : DataObjectContractAttribute
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static class Repository1
        {
            [TestDataContract]
            public interface IProduct
            {
                int Id { get; set; }
                [PropertyContract.Required, PropertyContract.Validation.MaxLength(100)]
                string Name { get; set; }
                [PropertyContract.Validation.Range(0, 1000000)]
                decimal Price { get; set; }
            }
            [TestDataContract]
            public interface IOrder
            {
                int Id { get; set; }
                DateTime PlacedAt { get; set; }
                [PropertyContract.Required, PropertyContract.Semantic.EmailAddress]
                string CustomerEmail { get; set; }
                [PropertyContract.Required]
                ICollection<IOrderLine> OrderLines { get; }
                [PropertyContract.DefaultValue(OrderStatus.New)]
                OrderStatus Status { get; set; }
            }
            [TestDataContract]
            public interface IOrderLine
            {
                int Id { get; set; }
                [PropertyContract.Required]
                IOrder Order { get; set; }
                [PropertyContract.Required]
                IProduct Product { get; set; }
                [PropertyContract.Validation.Range(1, 1000)]
                int Quantity { get; set; }
            }
            public enum OrderStatus
            {
                New = 1,
                PaymentReceived = 2,
                ProductsShipped = 3
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static class Repository2
        {
            [TestDataContract]
            public interface IPrimaryContract
            {
                string PrimaryProperty { get; set; }
            }
            [TestDataContract]
            public interface IFirstMixinContract
            {
                int FirstMixinProperty { get; set; }
            }
            [TestDataContract]
            public interface ISecondMixinContract
            {
                TimeSpan SecondMixinPropertyA { get; set; }
                DateTime SecondMixinPropertyB { get; set; }
            }
        }
    }
}