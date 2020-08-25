using System;

namespace Twilight.ES.Contracts
{
    /// <summary>
    ///     Represents a domain aggregate.
    /// </summary>
    public interface IAggregate
    {
        /// <summary>
        ///     Gets the aggregate identifier.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        Guid AggregateId { get; }
    }
}
