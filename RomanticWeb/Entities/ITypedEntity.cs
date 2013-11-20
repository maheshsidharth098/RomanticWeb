using System.Collections.Generic;
using RomanticWeb.Mapping.Attributes;

namespace RomanticWeb.Entities
{
    /// <summary>A typed entity, ie. one that exists in a rdf:type relation.</summary>
    [Class("rdfs","Class")]
    public interface ITypedEntity:IEntity
    {
        /// <summary>Gets or sets the entity's rdf classes.</summary>
        [Property("rdf","type",IsCollection=true)]
        IEnumerable<EntityId> Types { get; set; }
    }
}