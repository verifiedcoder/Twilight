using System;
using Taikandi;
using Twilight.ES.Contracts;

namespace Twilight.ES
{
    /// <summary>
    ///     <para>Represents a domain aggregate.</para>
    ///     <para>Implements <see cref="IAggregate" />.</para>
    /// </summary>
    /// <seealso cref="IAggregate" />
    public abstract class Aggregate : IAggregate
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Aggregate" /> class.
        /// </summary>
        /// <remarks>An aggregate identifier is automatically assigned.</remarks>
        protected Aggregate() => AggregateId = SequentialGuid.NewGuid();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Aggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        protected Aggregate(Guid aggregateId) => AggregateId = aggregateId;

        /// <inheritdoc />
        public Guid AggregateId { get; }
    }
}
