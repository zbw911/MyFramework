 

using System;
using System.Linq.Expressions;

namespace Kt.Framework.Repository.Specifications
{
    /// <summary>
    /// The <see cref="ISpecification{TEntity}"/> interface defines a basic contract to express specifications declaratively.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the expression that encapsulates the criteria of the specification.
        /// </summary>
        Expression<Func<T, bool>> Predicate { get; }

        /// <summary>
        /// Evaluates the specification against an entity of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="T"/> instance to evaluate the specificaton
        /// against.</param>
        /// <returns>Should return true if the specification was satisfied by the entity, else false. </returns>
        bool IsSatisfiedBy(T entity);
    }
}