﻿using System;
using System.Collections;
using System.Globalization;
using RomanticWeb.Entities;
using RomanticWeb.Model;

namespace RomanticWeb.Converters
{
    /// <summary>Default converter for <see cref="Node"/>s to value objects or entities.</summary>
    public sealed class FallbackNodeConverter : IFallbackNodeConverter
    {
        private readonly IConverterCatalog _converters;

        /// <summary>Constructor with entity context passed.</summary>
        public FallbackNodeConverter(IConverterCatalog converters)
        {
            _converters = converters;
        }

        /// <summary>
        /// Converts a node to a correct value based on type (URI, blank or literal)
        /// or it's datatype in the case of literal nodes
        /// </summary>
        public object Convert(Node objectNode, IEntityContext context)
        {
            if (objectNode.IsLiteral)
            {
                return ConvertLiteral(objectNode, context);
            }

            return ConvertUri(objectNode, context);
        }

        /// <inheritdoc/>
        public Node ConvertBack(object value, IEntityContext context)
        {
            if (value is IEntity)
            {
                return Node.FromEntityId(((IEntity)value).Id);
            }

            if (value is IEnumerable && !(value is string))
            {
                throw new InvalidOperationException();
            }

            return ConvertOneBack(value, context);
        }

        private static Node ConvertOneBack(object element, IEntityContext context)
        {
            if (element is IEntity)
            {
                return Node.FromEntityId(((IEntity)element).Id);
            }

            if (element is Uri)
            {
                return Node.ForUri((Uri)element);
            }

            if (element is string)
            {
                return (context.CurrentCulture.Equals(CultureInfo.InvariantCulture) ?
                    Node.ForLiteral((string)element) :
                    Node.ForLiteral((string)element, context.CurrentCulture.TwoLetterISOLanguageName));
            }

            return Node.ForLiteral(element.ToString());
        }

        private object ConvertLiteral(Node objectNode, IEntityContext context)
        {
            var converter = _converters.GetBestConverter(objectNode);
            if (converter != null)
            {
                return converter.Convert(objectNode, context);
            }

            throw new InvalidOperationException(string.Format("No suitable converter found to convert node '{0}'", objectNode));
        }

        private object ConvertUri(Node uriNode, IEntityContext context)
        {
            return context.Load<IEntity>(uriNode.ToEntityId());
        }
    }
}