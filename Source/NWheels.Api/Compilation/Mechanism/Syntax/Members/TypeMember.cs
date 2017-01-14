﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;

namespace NWheels.Compilation.Mechanism.Syntax.Members
{
    public class TypeMember : AbstractMember
    {
        private TypeMember()
        {
            this.Interfaces = new HashSet<TypeMember>();
            this.GenericTypeArguments = new List<TypeMember>();
            this.Members = new List<AbstractMember>();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public TypeMember(Type compiledType)
            : this()
        {
            this.Binding = compiledType;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public TypeMember(ITypeGenerator generator)
            : this()
        {
            this.Generator = generator;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string FullName
        {
            get
            {
                return Namespace + "." + Name;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public TypeMember BaseType { get; set; }
        public HashSet<TypeMember> Interfaces { get; private set; }
        public TypeMemberKind TypeKind { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsValueType { get; set; }
        public bool IsArray { get; set; }
        public bool IsNullable { get; set; }
        public bool IsGenericType { get; set; }
        public bool IsGenericTypeDefinition { get; set; }
        public TypeMember GenericTypeDefinition { get; set; }
        public List<TypeMember> GenericTypeArguments { get; private set; }
        public TypeMember UnderlyingType { get; set; }
        public Type Binding { get; set; }
        public List<AbstractMember> Members { get; private set; }
        public ITypeGenerator Generator { get; private set; }
    }
}
