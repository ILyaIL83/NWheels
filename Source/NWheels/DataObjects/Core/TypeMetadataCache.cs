﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NWheels.DataObjects.Core
{
    public class TypeMetadataCache : ITypeMetadataCache
    {
        private readonly MetadataConventionSet _conventions;
        private readonly Dictionary<Type, MixinRegistration[]> _mixinsByPrimaryContract;
        private readonly Dictionary<Type, ConcretizationRegistration> _concretizationsByPrimaryContract;
        private readonly ConcurrentDictionary<Type, TypeMetadataBuilder> _metadataByContractType = new ConcurrentDictionary<Type, TypeMetadataBuilder>();
        private readonly ConcurrentDictionary<Type, ISemanticDataType> _semanticDataTypes;
        private readonly ConcurrentDictionary<Type, IStorageDataType> _storageDataTypes;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public TypeMetadataCache(
            MetadataConventionSet conventions,
            IEnumerable<MixinRegistration> mixinRegistrations,
            IEnumerable<ConcretizationRegistration> concretizationRegistrations)
        {
            _conventions = conventions;
            _conventions.InjectCache(this);

            _semanticDataTypes = new ConcurrentDictionary<Type, ISemanticDataType>();
            _storageDataTypes = new ConcurrentDictionary<Type, IStorageDataType>();
            _mixinsByPrimaryContract = mixinRegistrations.GroupBy(r => r.TargetContract).ToDictionary(g => g.Key, g => g.ToArray());
            _concretizationsByPrimaryContract = concretizationRegistrations.ToDictionary(r => r.GeneralContract);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public TypeMetadataCache(MetadataConventionSet conventions)
            : this(conventions, new MixinRegistration[0], new ConcretizationRegistration[0])
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ITypeMetadata GetTypeMetadata(Type primaryContract)
        {
            return _metadataByContractType.GetOrAdd(primaryContract, BuildTypeMetadata);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void EnsureRelationalMapping(ITypeMetadata type)
        {
            var metadata = (TypeMetadataBuilder)type;
            metadata.EnsureRelationalMapping(_conventions);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ISemanticDataType GetSemanticTypeInstance(Type semanticDataType, Type propertyClrType)
        {
            Type closedSemanticDataType = (
                semanticDataType.IsGenericType && semanticDataType.IsGenericTypeDefinition ?
                semanticDataType.MakeGenericType(propertyClrType) :
                semanticDataType);

            return _semanticDataTypes.GetOrAdd(closedSemanticDataType, key => (ISemanticDataType)Activator.CreateInstance(key));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public IStorageDataType GetStorageTypeInstance(Type storageDataType, Type propertyClrType)
        {
            Type closedStorageDataType = (
                storageDataType.IsGenericType && storageDataType.IsGenericTypeDefinition ?
                storageDataType.MakeGenericType(propertyClrType) :
                storageDataType);

            return _storageDataTypes.GetOrAdd(closedStorageDataType, key => (IStorageDataType)Activator.CreateInstance(key));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public MetadataConventionSet Conventions
        {
            get { return _conventions; }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal Snapshot TakeSnapshot()
        {
            return Snapshot.Create(this);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal TypeMetadataBuilder FindTypeMetadataAllowIncomplete(Type contract)
        {
            TypeMetadataBuilder metadata;
            var entriesBeingBuilt = _s_entriesBeingBuilt;

            if ( entriesBeingBuilt != null && entriesBeingBuilt.TryGetValue(contract, out metadata) )
            {
                return metadata;
            }

            return (TypeMetadataBuilder)GetTypeMetadata(contract);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private TypeMetadataBuilder BuildTypeMetadata(Type primaryContract)
        {
            var mixinContracts = GetRegisteredMixinContracts(primaryContract);
            return BuildTypeMetadata(primaryContract, mixinContracts);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private TypeMetadataBuilder BuildTypeMetadata(Type primaryContract, Type[] mixinContracts)
        {
            var entriesBeingBuilt = _s_entriesBeingBuilt;
            var ownEntriesBeingBuilt = (entriesBeingBuilt == null);

            if ( ownEntriesBeingBuilt )
            {
                entriesBeingBuilt = new Dictionary<Type, TypeMetadataBuilder>();
                _s_entriesBeingBuilt = entriesBeingBuilt;
            }

            try
            {
                var builder = new TypeMetadataBuilder();
                entriesBeingBuilt.Add(primaryContract, builder);

                ConcretizationRegistration concretization;
                if ( _concretizationsByPrimaryContract.TryGetValue(primaryContract, out concretization) && concretization.ConcreteContract != primaryContract )
                {
                    var concretizationMixinContracts = GetRegisteredMixinContracts(concretization.ConcreteContract);
                    return BuildTypeMetadata(
                        concretization.ConcreteContract, 
                        mixinContracts.Union(concretizationMixinContracts).ToArray());
                }
                else
                {
                    var constructor = new TypeMetadataBuilderConstructor(_conventions);
                    constructor.ConstructMetadata(primaryContract, mixinContracts, builder, cache: this);
                }

                return builder;
            }
            finally
            {
                if ( ownEntriesBeingBuilt )
                {
                    _s_entriesBeingBuilt = null;
                }
                else
                {
                    entriesBeingBuilt.Remove(primaryContract);
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private Type[] GetRegisteredMixinContracts(Type primaryContract)
        {
            MixinRegistration[] mixinRegistrations;

            if ( _mixinsByPrimaryContract.TryGetValue(primaryContract, out mixinRegistrations) )
            {
                return mixinRegistrations.Select(r => r.MixinContract).ToArray();
            }
            else
            {
                return Type.EmptyTypes;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [ThreadStatic]
        private static Dictionary<Type, TypeMetadataBuilder> _s_entriesBeingBuilt;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal class Snapshot
        {
            public Dictionary<string, object> Contracts { get; set; }
            public List<string> SemanticDataTypes { get; set; }
            public List<string> StorageDataTypes { get; set; }
            public List<string> Concretizations { get; set; }
            public List<string> Mixins { get; set; }
            public List<string> MetadataConventions { get; set; }
            public List<string> RelationalMappingConventions { get; set; }

            //-------------------------------------------------------------------------------------------------------------------------------------------------

            public static Snapshot Create(TypeMetadataCache cache)
            {
                return new Snapshot() {
                    Contracts = 
                        cache._metadataByContractType.ToDictionary(kvp => kvp.Key.FullName, kvp => (object)kvp.Value),
                    SemanticDataTypes = 
                        cache._semanticDataTypes.Values.Select(v => v.GetType().FullName).OrderBy(s => s).ToList(),
                    StorageDataTypes = 
                        cache._storageDataTypes.Values.Select(v => v.GetType().FullName).OrderBy(s => s).ToList(),
                    Concretizations = 
                        cache._concretizationsByPrimaryContract.Values
                        .Select(v => string.Format("{0} --|> {1}", v.ConcreteContract.FullName, v.GeneralContract.FullName))
                        .ToList(),
                    Mixins = 
                        cache._mixinsByPrimaryContract.Values
                        .SelectMany(r => r).Select(r => string.Format("{0} += {1}", r.TargetContract.FullName, r.MixinContract.FullName))
                        .ToList(),
                    MetadataConventions = 
                        cache._conventions.MetadataConventions.Select(c => c.GetType().FullName).OrderBy(s => s).ToList(),
                    RelationalMappingConventions = 
                        cache._conventions.RelationalMappingConventions.Select(c => c.GetType().FullName).OrderBy(s => s).ToList()
                };
            }
        }
    }
}