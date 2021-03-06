﻿using NWheels.Compilation.Mechanism.Factories;
using NWheels.Compilation.Mechanism.Syntax.Members;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace NWheels.Compilation.Mechanism.Factories
{
    public abstract class TypeFactoryBase<TContextExtension, TArtifact> : ITypeFactory
    {
        private readonly ITypeLibrary<TArtifact> _library;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected TypeFactoryBase(ITypeLibrary<TArtifact> library)
        {
            _library = library;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        Type ITypeFactory.ArtifactType => typeof(TArtifact);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected abstract void DefinePipelineAndExtendFactoryContext(
            TypeKey key,
            List<ITypeFactoryConvention> pipeline,
            out TContextExtension contextExtension);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected TypeMember GetOrBuildTypeMember(ref TypeKey key)
        {
            return _library.GetOrBuildTypeMember(key, BuildNewTypeMember);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected ITypeLibrary<TArtifact> Library => _library;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private TypeMember BuildNewTypeMember(TypeKey key)
        {
            var type = new TypeMember(new TypeGeneratorInfo(this.GetType(), key));

            _library.DeclareTypeMember(key, type);

            var conventionPipeline = new List<ITypeFactoryConvention>();

            DefinePipelineAndExtendFactoryContext(
                key,
                conventionPipeline,
                out TContextExtension contextExtension);

            var factoryContext = _library.CreateFactoryContext<TContextExtension>(key, type, contextExtension);
            ExecuteConventionPipeline(conventionPipeline, factoryContext);

            return type;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private void ExecuteConventionPipeline(List<ITypeFactoryConvention> pipeline, ITypeFactoryContext factoryContext)
        {
            var effectivePipeline = pipeline.Where(sink => sink.ShouldApply(factoryContext)).ToImmutableList();

            foreach (var sink in effectivePipeline)
            {
                sink.Validate(factoryContext);
            }

            foreach (var sink in effectivePipeline)
            {
                sink.Declare(factoryContext);
            }

            foreach (var sink in effectivePipeline)
            {
                sink.Implement(factoryContext);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------

    public abstract class TypeFactoryBase<TArtifact> : TypeFactoryBase<Empty.ContextExtension, TArtifact>
    {
        protected TypeFactoryBase(ITypeLibrary<TArtifact> mechanism)
            : base(mechanism)
        {
        }
    }
}
