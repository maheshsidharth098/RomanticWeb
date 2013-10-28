using System;
using System.Collections.Generic;
using System.Linq;
using RomanticWeb.Entities;
using RomanticWeb.Ontologies;

namespace RomanticWeb
{
    /// <summary>Defines methods for factories, which produce <see cref="Entity"/> instances.</summary>
    /// todo: less method overloads
    /// todo: drop generic constraints
    /// todo: IDisposable?
    public interface IEntityContext
    {
        /// <summary>Enables given entity factory to be LINQ queryable with respect to the underlying triple store.</summary>
        /// <returns>A queryable collection of entities.</returns>
        IQueryable<Entity> AsQueryable();

        /// <summary>Enables given entity factory to be LINQ queryable with respect to the underlying triple store.</summary>
        /// <typeparam name="T">Type to be used when returning a typed entity.</typeparam>
        /// <returns>A queryable collection of typed entities.</returns>
        IQueryable<T> AsQueryable<T>() where T : class,IEntity;

        /// <summary>Loads an existing entity.</summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="checkIfExist"></param>
        /// <returns>Instance of an entity wih given identifier or null.</returns>
        Entity Load(EntityId entityId,bool checkIfExist=true);

        /// <summary>Loads an existing typed entity.</summary>
        /// <typeparam name="T">Type to be used when returning a typed entity.</typeparam>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="checkIfExist"></param>
        /// <returns>Typed instance of an entity wih given identifier or null.</returns>
        T Load<T>(EntityId entityId,bool checkIfExist=true) where T : class,IEntity;

        /// <summary>Creates a new typed entity.</summary>
        /// <typeparam name="T">Type to be used when returning a typed entity.</typeparam>
        /// <param name="entityId">Entity identifier</param>
        T Create<T>(EntityId entityId) where T : class,IEntity;

        /// <summary>Creates a new entity.</summary>
        /// <param name="entityId">Entity identifier</param>
        Entity Create(EntityId entityId);

        /// <summary>Load an enumerable collection of entities beeing a result of a SPARQL construct query.</summary>
        /// <param name="sparqlConstruct">SPARQL construct query.</param>
        /// <returns>An enumerable collection of entities beeing a result of a SPARQL construct query</returns>
        IEnumerable<Entity> Load(string sparqlConstruct);

        /// <summary>Load an enumerable collection of typed entities beeing a result of a SPARQL construct query.</summary>
        /// <typeparam name="T">Type to be used when returning a typed entity.</typeparam>
        /// <param name="sparqlConstruct">SPARQL construct query.</param>
        /// <returns>An enumerable collection of typed entities beeing a result of a SPARQL construct query</returns>
        IEnumerable<T> Load<T>(string sparqlConstruct) where T:class,IEntity;
    }
}