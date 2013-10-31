﻿using System;
using NullGuard;

namespace RomanticWeb.Mapping.Model
{
    internal class PropertyMapping : IPropertyMapping
	{
        public PropertyMapping(Type returnType,string name,Uri uri,[AllowNull] IGraphSelectionStrategy graphSelector,bool isCollection)
        {
            ReturnType=returnType;
            IsCollection=isCollection;
            Name=name;
            Uri=uri;
            GraphSelector=graphSelector??new NullGraphSelector();
        }

        public Uri Uri { get; private set; }

		public IGraphSelectionStrategy GraphSelector { get; private set; }

        public string Name { get; private set; }

        public bool IsCollection { get; private set; }

        public Type ReturnType { get; private set; }
	}
}