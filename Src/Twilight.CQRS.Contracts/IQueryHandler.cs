using System.Threading;
using System.Threading.Tasks;

namespace Twilight.CQRS.Contracts
{
    /// <summary>
    ///     Represents a query message handler.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> : IMessageHandler<TQuery>
        where TQuery : class, IQuery<TResponse>
    {
        /// <summary>
        ///     Handles the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <para>A task that represents the asynchronous query handler operation.</para>
        ///     <para>The task result contains the query execution response.</para>
        /// </returns>
        Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
