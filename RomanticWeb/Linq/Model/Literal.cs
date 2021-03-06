﻿using System;
using System.Globalization;
using NullGuard;

namespace RomanticWeb.Linq.Model
{
    /// <summary>Expresses a literal in the query.</summary>
    public class Literal : QueryComponent, IExpression
    {
        #region Fields
        private object _value;
        private Type _valueType;
        #endregion

        #region Constructors
        /// <summary>Constructor for creating <b>null</b> literals of given type.</summary>
        /// <param name="valueType"></param>
        public Literal(Type valueType)
            : base()
        {
            _valueType = valueType;
        }

        /// <summary>Base constructor with value passed.</summary>
        /// <param name="value">Value of this literal.</param>
        public Literal(object value)
            : base()
        {
            _valueType = (_value = value).GetType();
        }
        #endregion

        #region Properties
        /// <summary>Gets a value of this literal.</summary>
        public object Value { [return: AllowNull] get { return _value; } }

        /// <summary>Gets the type of value.</summary>
        public Type ValueType { get { return _valueType; } }
        #endregion

        #region Public methods
        /// <summary>Creates a string representation of this literal.</summary>
        /// <returns>String representation of this literal.</returns>
        public override string ToString()
        {
            string valueString = System.String.Empty;
            if (_value != null)
            {
                switch (_value.GetType().FullName)
                {
                    default:
                    case "System.TimeSpan":
                    case "System.String":
                        valueString = System.String.Format("\"{0}\"", _value);
                        break;
                    case "System.Byte":
                    case "System.SByte":
                    case "System.Int16":
                    case "System.UInt16":
                    case "System.Int32":
                    case "System.UInt32":
                    case "System.Int64":
                    case "System.UInt64":
                        valueString = _value.ToString();
                        break;
                    case "System.Char":
                        valueString = System.String.Format("'{0}'", _value);
                        break;
                    case "System.Single":
                    case "System.Double":
                    case "System.Decimal":
                    case "System.DateTime":
                        valueString = System.String.Format(CultureInfo.InvariantCulture, "{0}", _value);
                        break;
                    case "RomanticWeb.Entities.EntityId":
                    case "System.Uri":
                        valueString = System.String.Format("<{0}>", _value);
                        break;
                }
            }

            return valueString;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="operand">Type: <see cref="System.Object" />
        /// The object to compare with the current object.</param>
        /// <returns>Type: <see cref="System.Boolean" />
        /// <b>true</b> if the specified object is equal to the current object; otherwise, <b>false</b>.</returns>
        public override bool Equals([AllowNull] object operand)
        {
            if (Object.Equals(operand, null)) { return false; }
            if (operand.GetType() != typeof(Literal)) { return false; }
            if (_value != null)
            {
                if (_value is Uri)
                {
                    return AbsoluteUriComparer.Default.Equals((Uri)_value, (Uri)((Literal)operand)._value);
                }
                else
                {
                    return _value.Equals(((Literal)operand)._value);
                }
            }
            else
            {
                return Object.Equals(((Literal)operand)._value, null);
            }
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>Type: <see cref="System.Int32" />
        /// A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return typeof(Literal).FullName.GetHashCode() ^ (_value != null ? (_value is Uri ? _value.ToString().GetHashCode() : _value.GetHashCode()) : 0);
        }
        #endregion
    }
}