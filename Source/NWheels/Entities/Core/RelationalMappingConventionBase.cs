﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using NWheels.DataObjects;
using NWheels.DataObjects.Core;

namespace NWheels.Entities.Core
{
    public abstract class RelationalMappingConventionBase : IRelationalMappingConvention
    {
        private PluralizationService _pluralizationService;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        #region IRelationalMappingConvention Members

        void IRelationalMappingConvention.Preview(ITypeMetadata type)
        {
            PreviewType((TypeMetadataBuilder)type);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        void IRelationalMappingConvention.Apply(ITypeMetadata type)
        {
            ApplyToType((TypeMetadataBuilder)type);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        void IRelationalMappingConvention.Finalize(ITypeMetadata type)
        {
            FinalizeType((TypeMetadataBuilder)type);
        }

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual void PreviewType(TypeMetadataBuilder type)
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual void FinalizeType(TypeMetadataBuilder type)
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual void ApplyToType(TypeMetadataBuilder type)
        {
            type.RelationalMapping = new TypeRelationalMappingBuilder() {
                PrimaryTableName = NameTypePrimaryTable(type),
                InheritanceKind = GetInheritanceKind(type)
            };

            var keyProperties = new HashSet<PropertyMetadataBuilder>();

            foreach ( var key in type.AllKeys )
            {
                foreach ( var property in key.Properties )
                {
                    keyProperties.Add(property);

                    property.RelationalMapping = new PropertyRelationalMappingBuilder() {
                        TableName = NameKeyPropertyColumnTable(type, key, property),
                        ColumnName = NameKeyPropertyColumn(type, key, property),
                        DataTypeName = NameKeyPropertyColumnDataType(type, key, property)
                    };
                }
            }

            foreach ( var property in type.Properties.Where(p => !keyProperties.Contains(p)) )
            {
                property.RelationalMapping = new PropertyRelationalMappingBuilder() {
                    TableName = NamePropertyColumnTable(type, property),
                    ColumnName = NamePropertyColumn(type, property),
                    DataTypeName = NamePropertyColumnDataType(type, property)
                };
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected abstract string NameTypePrimaryTable(TypeMetadataBuilder type);
        protected abstract string NamePropertyColumnTable(TypeMetadataBuilder type, PropertyMetadataBuilder property);
        protected abstract string NamePropertyColumn(TypeMetadataBuilder type, PropertyMetadataBuilder property);
        protected abstract string NamePropertyColumnDataType(TypeMetadataBuilder type, PropertyMetadataBuilder property);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual RelationalInheritanceKind? GetInheritanceKind(TypeMetadataBuilder type)
        {
            return RelationalInheritanceKind.TablePerHierarchy;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string NameKeyPropertyColumnDataType(TypeMetadataBuilder type, KeyMetadataBuilder key, PropertyMetadataBuilder property)
        {
            return NamePropertyColumnDataType(type, property);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string NameKeyPropertyColumn(TypeMetadataBuilder type, KeyMetadataBuilder key, PropertyMetadataBuilder property)
        {
            return NamePropertyColumn(type, property);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string NameKeyPropertyColumnTable(TypeMetadataBuilder type, KeyMetadataBuilder key, PropertyMetadataBuilder property)
        {
            return NamePropertyColumnTable(type, property);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected PluralizationService PluralizationService
        {
            get
            {
                if ( _pluralizationService == null )
                {
                    _pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
                }

                return _pluralizationService;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal static IRelationalMappingConvention FromDefault(RelationalMappingConventionDefault @default)
        {
            switch ( @default.Type )
            {
                case RelationalMappingConventionDefault.ConventionType.PascalCase:
                    return new PascalCaseRelationalMappingConvention(@default.UsePluralTableNames);
                case RelationalMappingConventionDefault.ConventionType.Underscore:
                    return new UnderscoreRelationalMappingConvention(@default.UsePluralTableNames);
                default:
                    throw new ArgumentOutOfRangeException("default.Type");
            }
        }
    }
}
